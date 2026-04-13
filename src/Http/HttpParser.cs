namespace Torff.Http
{
    public class HttpParser
    {
        public static HttpRequest Parse(string rawRequest)
        {
            if (string.IsNullOrWhiteSpace(rawRequest)) return null;

            var request = new HttpRequest();

            string[] sections = rawRequest.Split(new[] { "\r\n\r\n", "\n\n" }, 2, StringSplitOptions.None);
            
            string headerSection = sections[0];
            
            request.Body = sections.Length > 1 ? sections[1].TrimEnd('\0') : "";

            string[] lines = headerSection.Split('\n');
            string firstLine = lines[0].Trim();
            string[] parts = firstLine.Split(' ');

            if (parts.Length < 3) return null;

            request.Method = parts[0];
            string rawPath = parts[1];

            if (rawPath.Contains("?"))
            {
                string[] pathParts = rawPath.Split('?');
                request.Path = pathParts[0]; 
                string[] queryPairs = pathParts[1].Split('&');
                foreach (var pair in queryPairs)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                        request.QueryParameters[Uri.UnescapeDataString(keyValue[0])] = Uri.UnescapeDataString(keyValue[1]);
                }
            }
            else
            {
                request.Path = rawPath;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    string headerName = line.Substring(0, colonIndex).Trim();
                    string headerValue = line.Substring(colonIndex + 1).Trim();
                    request.Headers[headerName] = headerValue;
                }
            }

            return request;
        }
    }
}
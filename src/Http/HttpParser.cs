namespace Torff.Http
{
    public class HttpParser
    {
        public static HttpRequest Parse(string rawRequest)
        {
            if (string.IsNullOrWhiteSpace(rawRequest)) return null;

            string[] lines = rawRequest.Split('\n');
            string firstLine = lines[0].Trim();
            string[] parts = firstLine.Split(' ');

            if (parts.Length < 3) return null;

            var request = new HttpRequest
            {
                Method = parts[0],
                QueryParameters = new Dictionary<string, string>() 
            };

            string rawPath = parts[1];

            if (rawPath.Contains("?"))
            {
                string[] pathParts = rawPath.Split('?');
                request.Path = pathParts[0]; 
                
                string queryString = pathParts[1]; 

                string[] queryPairs = queryString.Split('&');
                foreach (var pair in queryPairs)
                {
                    string[] keyValue = pair.Split('=');
                    
                    if (keyValue.Length == 2)
                    {
                        string key = Uri.UnescapeDataString(keyValue[0]);
                        string value = Uri.UnescapeDataString(keyValue[1]);
                        request.QueryParameters[key] = value;
                    }
                }
            }
            else
            {
                request.Path = rawPath;
            }

            return request;
        }
    }
}
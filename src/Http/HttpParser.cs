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

            return new HttpRequest
            {
                Method = parts[0], // "GET"
                Path = parts[1]    // "/index.html"
            };
        }
    }
}
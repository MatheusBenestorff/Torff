using System.Text;

namespace Torff.Http
{
    public class HttpResponse
    {
        public string Version { get; set; } = "HTTP/1.1";
        public string StatusCode { get; set; } = "200 OK";
        public string ContentType { get; set; } = "text/html; charset=UTF-8";
        public string Body { get; set; } = "";

        public override string ToString()
        {
            StringBuilder responseBuilder = new StringBuilder();

            // Status
            responseBuilder.Append($"{Version} {StatusCode}\r\n");

            // Headers
            responseBuilder.Append($"Content-Type: {ContentType}\r\n");

            int contentLength = Encoding.UTF8.GetByteCount(Body);
            responseBuilder.Append($"Content-Length: {contentLength}\r\n");

            responseBuilder.Append("\r\n");

            // Body
            responseBuilder.Append(Body);

            return responseBuilder.ToString();
        }
    }
}
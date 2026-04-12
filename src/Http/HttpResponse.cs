using System.Text;

namespace Torff.Http
{
    public class HttpResponse
    {
        public string Version { get; set; } = "HTTP/1.1";
        public string StatusCode { get; set; } = "200 OK";
        public string ContentType { get; set; } = "text/html; charset=UTF-8";
        public byte[] BodyData { get; set; } = new byte[0];

        public byte[] GetBytes()
        {
            StringBuilder headersBuilder = new StringBuilder();

            headersBuilder.Append($"{Version} {StatusCode}\r\n");
            headersBuilder.Append($"Content-Type: {ContentType}\r\n");
            headersBuilder.Append($"Content-Length: {BodyData.Length}\r\n");
            headersBuilder.Append("\r\n");

            byte[] headersBytes = Encoding.UTF8.GetBytes(headersBuilder.ToString());

            byte[] fullResponse = new byte[headersBytes.Length + BodyData.Length];

            Buffer.BlockCopy(headersBytes, 0, fullResponse, 0, headersBytes.Length);
            Buffer.BlockCopy(BodyData, 0, fullResponse, headersBytes.Length, BodyData.Length);

            return fullResponse;
        }
    }
}
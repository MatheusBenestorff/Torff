
using System.Text;

namespace Torff.Http
{
    public class HttpResponse
    {
        public string Version { get; set; } = "HTTP/1.1";
        public string StatusCode { get; set; } = "200 OK";
        public string ContentType { get; set; } = "text/html; charset=UTF-8";
        
        public byte[] BodyData { get; set; } = new byte[0];
        
        public string FilePath { get; set; } 
        
        public bool KeepAlive { get; set; } = true;

        public async Task SendAsync(Stream networkStream)
        {
            StringBuilder headers = new StringBuilder();
            headers.Append($"{Version} {StatusCode}\r\n");
            headers.Append($"Content-Type: {ContentType}\r\n");
            
            string connectionStatus = KeepAlive ? "keep-alive" : "close";
            headers.Append($"Connection: {connectionStatus}\r\n");

            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                headers.Append("Transfer-Encoding: chunked\r\n\r\n");
                
                byte[] headerBytes = Encoding.UTF8.GetBytes(headers.ToString());
                await networkStream.WriteAsync(headerBytes, 0, headerBytes.Length);

                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[8192]; 
                    int bytesRead;

                    while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string hexSize = bytesRead.ToString("X") + "\r\n";
                        byte[] hexBytes = Encoding.UTF8.GetBytes(hexSize);
                        await networkStream.WriteAsync(hexBytes, 0, hexBytes.Length);

                        await networkStream.WriteAsync(buffer, 0, bytesRead);

                        byte[] crlf = Encoding.UTF8.GetBytes("\r\n");
                        await networkStream.WriteAsync(crlf, 0, crlf.Length);
                    }
                }

                byte[] endChunk = Encoding.UTF8.GetBytes("0\r\n\r\n");
                await networkStream.WriteAsync(endChunk, 0, endChunk.Length);
            }
            else
            {
                headers.Append($"Content-Length: {BodyData.Length}\r\n\r\n");
                
                byte[] headerBytes = Encoding.UTF8.GetBytes(headers.ToString());
                await networkStream.WriteAsync(headerBytes, 0, headerBytes.Length);
                await networkStream.WriteAsync(BodyData, 0, BodyData.Length);
            }
        }
    }
}
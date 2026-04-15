using System.Text;
using Torff.Http;
using Torff.Server.Adapters; 
using System.Net.Sockets;

namespace Torff.Routing
{
    public class Router
    {
        private readonly string _baseDirectory;

        public Router(string webRootFolder)
        {
            _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), webRootFolder);
        }

        public HttpResponse Route(HttpRequest request, string clientIp)
        {
            if (request.Path.StartsWith("/api"))
            {
                var ttpRequest = TtpAdapter.ConvertToTtp(request, clientIp);

                try
                {
                    using (TcpClient upstreamClient = new TcpClient())
                    {
                        // Framework port
                        upstreamClient.Connect("127.0.0.1", 5000);

                        throw new NotImplementedException("Connection established, but the TTP transmission has not yet been encrypted.");
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine($"[Torff-Proxy] Upstream on port 5000 is down. Returning a 502 error.");
                    
                    return new HttpResponse
                    {
                        StatusCode = "502 Bad Gateway",
                        ContentType = "text/html; charset=UTF-8",
                        BodyData = Encoding.UTF8.GetBytes("<h1>502 Bad Gateway</h1><p>Torff Server: The target framework is not responding or is offline.</p>")
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Torff-Proxy] Internal gateway error: {ex.Message}");
                    
                    return new HttpResponse
                    {
                        StatusCode = "500 Internal Server Error",
                        ContentType = "text/html; charset=UTF-8",
                        BodyData = Encoding.UTF8.GetBytes("<h1>500 Internal Server Error</h1>")
                    };
                }
            }

            string requestedFile = request.Path == "/" ? "index.html" : request.Path.TrimStart('/');

            string filePath = Path.Combine(_baseDirectory, requestedFile);

            if (File.Exists(filePath))
            {

                string extension = Path.GetExtension(filePath).ToLower();
                string contentType = GetContentType(extension);

                return new HttpResponse
                {
                    StatusCode = "200 OK",
                    ContentType = contentType,
                    FilePath = filePath
                };
            }

            return new HttpResponse
            {
                StatusCode = "404 Not Found",
                ContentType = "text/html; charset=UTF-8",
                BodyData = Encoding.UTF8.GetBytes("<h1>404</h1><p>Not Found.</p>")
            };
            
        }

        private string GetContentType(string extension)
        {
            return extension switch
            {
                ".html" => "text/html; charset=UTF-8",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".ico" => "image/x-icon",
                _ => "application/octet-stream",
            };
        }
    }
}
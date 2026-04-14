using System.Text;
using Torff.Http;
using Torff.Server.Adapters; 
using Torff.Server.Mocks;

namespace Torff.Routing
{
    public class Router
    {
        private readonly string _baseDirectory;
        private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _apiRoutes;

        public Router(string webRootFolder)
        {
            _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), webRootFolder);
            _apiRoutes = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        }

        public HttpResponse Route(HttpRequest request, string clientIp)
        {
            if (request.Path.StartsWith("/api"))
            {
                var ttpRequest = TtpAdapter.ConvertToTtp(request, clientIp);

                var ttpResponse = MockFramework.Processar(ttpRequest);

                return TtpAdapter.ConvertToHttp(ttpResponse);
            }

            string requestedFile = request.Path == "/" ? "index.html" : request.Path.TrimStart('/');

            string filePath = Path.Combine(_baseDirectory, requestedFile);

            if (File.Exists(filePath))
            {
                byte[] fileContent = File.ReadAllBytes(filePath);

                string extension = Path.GetExtension(filePath).ToLower();
                string contentType = GetContentType(extension);

                return new HttpResponse
                {
                    StatusCode = "200 OK",
                    ContentType = contentType,
                    BodyData = fileContent
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
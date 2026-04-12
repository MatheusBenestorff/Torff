using System.Text;
using Torff.Http;

namespace Torff.Routing
{
    public class Router
    {
        private readonly string _baseDirectory;

        public Router()
        {
            _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public HttpResponse Route(HttpRequest request)
        {
            string requestedFile = request.Path == "/" ? "index.html" : request.Path.TrimStart('/');

            string filePath = Path.Combine(_baseDirectory, requestedFile);

            if (File.Exists(filePath))
            {
                byte[] fileContent = File.ReadAllBytes(filePath);

                // Pega a extensão (ex: ".png") e descobre o tipo
                string extension = Path.GetExtension(filePath).ToLower();
                string contentType = GetContentType(extension);

                return new HttpResponse
                {
                    StatusCode = "200 OK",
                    ContentType = contentType,
                    BodyData = fileContent
                };
            }
            else
            {
                string notFoundHtml = "<h1>404 Error</h1><p>Page not found on the Torff server.</p>";
                return new HttpResponse
                {
                    StatusCode = "404 Not Found",
                    ContentType = "text/html; charset=UTF-8",
                    BodyData = Encoding.UTF8.GetBytes(notFoundHtml)
                };
            }
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
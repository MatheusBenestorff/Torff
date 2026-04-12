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
                string fileContent = File.ReadAllText(filePath);

                return new HttpResponse
                {
                    StatusCode = "200 OK",
                    Body = fileContent
                };
            }
            else
            {
                return new HttpResponse
                {
                    StatusCode = "404 Not Found",
                    Body = "<h1>404 Error</h1><p>Page not found on the Torff server.</p>"
                };
            }
        }
    }
}
using System.Net;
using System.Net.Sockets;
using Torff.Http;
using Torff.Routing;

namespace Torff.Core
{
    public class Server
    {
        private readonly int _port;

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);

            listener.Start();
            Console.WriteLine($"[Torff] Server started and listening on port {_port}...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine($"\n[Torff] Connection Detected: {client.Client.RemoteEndPoint}");

                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string requestText = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);

                HttpRequest request = HttpParser.Parse(requestText);

                if (request != null)
                {
                    Console.WriteLine($"[Torff] Request processed: The browser wants to make a {request.Method} request to the path {request.Path}");

                    Router router = new Router();

                    HttpResponse response = router.Route(request);

                    byte[] responseBytes = response.GetBytes();
                    stream.Write(responseBytes, 0, responseBytes.Length);

                    Console.WriteLine($"[Torff] Response sent with Status: {response.StatusCode}");
                }

                client.Close();
            }
        }
    }
}
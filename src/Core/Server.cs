using System.Net;
using System.Net.Sockets;
using Torff.Http;

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

                    HttpResponse response = new HttpResponse
                    {
                        Body = $"<html><body style='font-family: sans-serif;'>" +
                            $"<h1>Hello, world! Torff is alive!</h1>" +
                            $"<p>You accessed the path: <strong>{request.Path}</strong></p>" +
                            $"</body></html>"
                    };

                    byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(response.ToString());

                    stream.Write(responseBytes, 0, responseBytes.Length);

                    Console.WriteLine("[Torff] Your response was sent successfully!");
                }

                client.Close();
            }
        }
    }
}
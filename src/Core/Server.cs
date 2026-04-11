using System.Net;
using System.Net.Sockets;

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

                Console.WriteLine(requestText);

                client.Close();            
            }
        }
    }
}
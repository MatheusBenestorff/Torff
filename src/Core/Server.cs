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

                client.Close();
            }
        }
    }
}
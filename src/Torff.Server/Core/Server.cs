using System.Net;
using System.Net.Sockets;
using Torff.Http;
using Torff.Routing;
using Torff.Config;

namespace Torff.Core
{
    public class Server
    {
        private readonly ServerConfig _config;

        public Server(ServerConfig config)
        {
            _config = config;
        }
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _config.Port);
            listener.Start();
            Console.WriteLine($"[Torff] Server started and listening on port {_config.Port}...");
            Console.WriteLine($"[Torff] Root Folder: {_config.WebRoot} | Keep-Alive: {_config.EnableKeepAlive}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"\n[Torff] New connection detected: {client.Client.RemoteEndPoint}");

                Task.Run(() => ProcessClient(client));
            }
        }

        private void ProcessClient(TcpClient client)
        {
            string clientIp = client.Client.RemoteEndPoint.ToString();

            try
            {
                NetworkStream stream = client.GetStream();

                stream.ReadTimeout = _config.TimeoutSeconds * 1000;
                bool keepAlive = _config.EnableKeepAlive;

                while (keepAlive)
                {
                    byte[] buffer = new byte[4096];
                    
                    int bytesRead = 0;

                    try 
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    } 
                    catch (IOException) 
                    {
                        Console.WriteLine($"[Torff] Timeout reached for {clientIp}. Ending inactivity.");
                        break; 
                    }

                    if (bytesRead == 0) break;

                    string requestText = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    HttpRequest request = HttpParser.Parse(requestText);

                    if (request != null)
                    {
                        if (request.Headers.ContainsKey("Connection") && request.Headers["Connection"].ToLower() == "close")
                        {
                            keepAlive = false;
                        }

                        Router router = new Router(_config.WebRoot);
                        HttpResponse response = router.Route(request, clientIp);
                        
                        response.KeepAlive = keepAlive;

                        byte[] responseBytes = response.GetBytes();
                        stream.Write(responseBytes, 0, responseBytes.Length);
                        
                        Console.WriteLine($"[Torff] Response sent ({response.StatusCode}) to {clientIp}. Keeping the connection open: {keepAlive}");
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Torff] Error processing client: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine($"[Torff] Connection to {clientIp} has been completely closed.\n");
            }
        }
    }
}
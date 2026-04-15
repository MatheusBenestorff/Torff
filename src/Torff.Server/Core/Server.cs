using System.Net;
using System.Net.Sockets;
using Torff.Http;
using Torff.Routing;
using Torff.Config;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Torff.Core
{
    public class Server
    {
        private readonly ServerConfig _config;
        private X509Certificate2? _certificate;

        public Server(ServerConfig config)
        {
            _config = config;

            if (_config.EnableHttps)
            {
                if (!File.Exists(_config.CertificatePath))
                {
                    throw new FileNotFoundException($"[Torff-Erro] Certificado não encontrado em: {_config.CertificatePath}");
                }
                _certificate = new X509Certificate2(_config.CertificatePath, _config.CertificatePassword);
            }
        }
        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _config.Port);
            listener.Start();
            Console.WriteLine($"[Torff] Server started and listening on port {_config.Port}...");
            Console.WriteLine($"[Torff] Root Folder: {_config.WebRoot} | Keep-Alive: {_config.EnableKeepAlive}");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                
                _ = ProcessClientAsync(client);
            }
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            string clientIp = client.Client.RemoteEndPoint.ToString();

            try
            {
                NetworkStream networkStream = client.GetStream();
                
                Stream stream = networkStream;

                stream.ReadTimeout = _config.TimeoutSeconds * 1000;

                if (_config.EnableHttps)
                {
                    var sslStream = new SslStream(networkStream, false);
                    await sslStream.AuthenticateAsServerAsync(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);
                    stream = sslStream; 
                }
                
                bool keepAlive = _config.EnableKeepAlive;

                while (keepAlive)
                {
                    byte[] buffer = new byte[4096];
                    
                    int bytesRead = 0;
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.TimeoutSeconds)))
                    {
                        try 
                        {
                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                        } 
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine($"[Torff] Timeout reached for {clientIp}. Ending inactivity.");
                            break; 
                        }
                        catch (Exception)
                        {
                            break;
                        }
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

                        await response.SendAsync(stream);
                        
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
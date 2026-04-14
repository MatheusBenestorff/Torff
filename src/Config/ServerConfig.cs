namespace Torff.Config
{
    public class ServerConfig
    {
        public int Port { get; set; } = 5000;
        public string WebRoot { get; set; } = "wwwroot";
        public bool EnableKeepAlive { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 5;
    }
}
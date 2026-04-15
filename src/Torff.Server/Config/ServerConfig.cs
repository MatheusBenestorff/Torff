namespace Torff.Config
{
    public class ServerConfig
    {
        public int Port { get; set; } = 5000;
        public string WebRoot { get; set; } = "wwwroot";
        public bool EnableKeepAlive { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 5;
        public bool EnableHttps { get; set; } = false;
        public string CertificatePath { get; set; } = "";
        public string CertificatePassword { get; set; } = "";
    }
}
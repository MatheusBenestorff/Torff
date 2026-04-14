namespace Torff.Ttp
{
    public class TtpRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString("N");
        
        // Routing data
        public string Method { get; set; } = "GET";
        public string Path { get; set; } = "/";
        
        public Dictionary<string, string> Query { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        
        public string Body { get; set; } = "";
        
        // Security
        public string ClientIp { get; set; } = "Unknown";
    }
}
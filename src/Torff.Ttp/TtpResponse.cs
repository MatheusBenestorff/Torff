namespace Torff.Ttp
{
    public class TtpResponse
    {
        public int StatusCode { get; set; } = 200;
        
        public string ContentType { get; set; } = "application/json"; 
        
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        
        public byte[] Body { get; set; } = new byte[0];
    }
}
using Torff.Http;
using Torff.Ttp;  

namespace Torff.Server.Adapters
{
    public class TtpAdapter
    {
        public static TtpRequest ConvertToTtp(HttpRequest originalRequest, string clientIp)
        {
            if (originalRequest == null) return null;

            var ttpRequest = new TtpRequest
            {
                Method = originalRequest.Method,
                Path = originalRequest.Path,
                Body = originalRequest.Body,
                ClientIp = clientIp,
                
                Query = new Dictionary<string, string>(originalRequest.QueryParameters),
                Headers = new Dictionary<string, string>(originalRequest.Headers),
            };

            return ttpRequest;
        }
    }
}
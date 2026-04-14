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

        public static HttpResponse ConvertToHttp(TtpResponse ttpResponse)
        {
            if (ttpResponse == null) return null;

            return new HttpResponse
            {
                StatusCode = GetStatusString(ttpResponse.StatusCode),
                ContentType = ttpResponse.ContentType,
                BodyData = ttpResponse.Body
            };
        }

        private static string GetStatusString(int code)
        {
            return code switch
            {
                200 => "200 OK",
                201 => "201 Created",
                400 => "400 Bad Request",
                404 => "404 Not Found",
                500 => "500 Internal Server Error",
                _ => $"{code} Unknown"
            };
        }
    }
}
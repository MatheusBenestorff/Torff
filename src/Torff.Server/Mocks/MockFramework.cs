using System;
using Torff.Ttp;

namespace Torff.Server.Mocks
{
    public class MockFramework
    {
        public static TtpResponse Processar(TtpRequest request)
        {
            Console.WriteLine($"\n[MOCK FRAMEWORK] Pedido recebido via TTP!");
            Console.WriteLine($"[MOCK FRAMEWORK] ID de Rastreio: {request.RequestId}");
            Console.WriteLine($"[MOCK FRAMEWORK] IP do Cliente: {request.ClientIp}");
            Console.WriteLine($"[MOCK FRAMEWORK] Rota Solicitada: {request.Method} {request.Path}");

            string jsonBody = $@"{{
                ""mensagem"": ""Olá! O protocolo TTP está vivo e respirando!"",
                ""requestId"": ""{request.RequestId}"",
                ""status"": ""Sucesso total""
            }}";

            return new TtpResponse
            {
                StatusCode = 200,
                ContentType = "application/json",
                Body = System.Text.Encoding.UTF8.GetBytes(jsonBody)
            };
        }
    }
}
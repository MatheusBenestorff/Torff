using Torff.Core;
using Torff.Config;

namespace Torff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the Torff Web Server...");
            ServerConfig config = ConfigLoader.Load("torff.json");
            
            Server server = new Server(config);

            server.Start();
        }
    }
}
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

            Core.Server server = new Core.Server(config);
            server.Start();
        }
    }
}
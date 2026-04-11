using Torff.Core;

namespace Torff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the Torff Web Server...");
            
            Server server = new Server(8080);
            
            server.Start();
        }
    }
}
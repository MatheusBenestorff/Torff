using System.Text.Json;

namespace Torff.Config
{
    public class ConfigLoader
    {
        public static ServerConfig Load(string filePath = "torff.json")
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Torff-Config] File '{filePath}' not found. Using default settings.");
                return new ServerConfig();
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                ServerConfig config = JsonSerializer.Deserialize<ServerConfig>(jsonString);
                
                Console.WriteLine("[Torff-Config] Settings loaded successfully!");
                return config ?? new ServerConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Torff-Config] Error reading JSON: {ex.Message}. Using default settings.");
                return new ServerConfig();
            }
        }
    }
}
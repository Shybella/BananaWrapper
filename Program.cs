using System;
using System.IO;
using System.Text.Json;
class Program
{
    static void Main(string[] args)
    {
        var configJson = File.ReadAllText("config.json");
        var config = JsonSerializer.Deserialize<Config>(configJson);

        Console.Title = config.ConsoleTitle;

        var serverWrapper = new MinecraftServerWrapper(config);

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (input == null) continue;

            if (input.Equals("start", StringComparison.OrdinalIgnoreCase))
            {
                serverWrapper.Start(autoRestart: true);
            }
            else if (input.Equals("stop", StringComparison.OrdinalIgnoreCase))
            {
                serverWrapper.Stop();
            }
            else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                if (serverWrapper.IsRunning())
                {
                    serverWrapper.Stop();
                }
                break;
            }
            else
            {
                serverWrapper.ExecuteCommand(input);
            }
        }
    }
}

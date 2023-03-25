using System.Text.Json;

//Config.cs

public class LoopingCommand
{
    public string Command { get; set; }
    public bool Loop { get; set; }
    public int Interval { get; set; }
}

public class Config
{
    public string ConsoleTitle { get; set; }
    public string JavaExecutable { get; set; }
    public string ServerJar { get; set; }
    public string JvmArguments { get; set; }
    public string[] Commands { get; set; }
    public List<LoopingCommand> LoopingCommands { get; set; }

    public static void CreateDefaultConfig()
    {
        var config = new Config
        {
            ConsoleTitle = "Minecraft Server Wrapper",
            JavaExecutable = "java",
            ServerJar = "server.jar",
            JvmArguments = "-Xmx2G -Xms2G",
            Commands = new string[] { "say Server started 5 minutes ago", "time set day" },
            LoopingCommands = new List<LoopingCommand>
            {
                new LoopingCommand { Command = "say This command will loop every 10 seconds", Loop = true, Interval = 10 },
                new LoopingCommand { Command = "say This command will only execute once", Loop = false, Interval = 0 }
            }
        };

        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("config.json", configJson);
    }
}

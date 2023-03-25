using System.Text.Json;

//Config.cs

public class CustomCommand
{
    public string Command { get; set; }
    public bool Loop { get; set; }
    public int Interval { get; set; }
    public int Delay { get; set; } // new property
}
public class Config
{
    public string ConsoleTitle { get; set; }
    public string JavaExecutable { get; set; }
    public string ServerJar { get; set; }
    public string JvmArguments { get; set; }
    public List<CustomCommand> Commands { get; set; }

    public static void CreateDefaultConfig()
    {
        var config = new Config
        {
            ConsoleTitle = "Minecraft Server Wrapper",
            JavaExecutable = "java",
            ServerJar = "server.jar",
            JvmArguments = "-Xmx2G -Xms2G",
            Commands = new List<CustomCommand>
            {
                new CustomCommand { Command = "say This command will run after 10 seconds", Loop = false, Interval = 0, Delay = 10 },
                new CustomCommand { Command = "say This command will only execute once", Loop = false, Interval = 0 }
            }
        };
        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("config.json", configJson);
    }
}

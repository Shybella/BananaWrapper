using System.Text.Json;

public class Config
{
    public string ConsoleTitle { get; set; }
    public string JavaExecutable { get; set; }
    public string ServerJar { get; set; }
    public string JvmArguments { get; set; }
    public string[] Commands { get; set; }

    public static void CreateDefaultConfig()
    {
        var config = new Config
        {
            ConsoleTitle = "Minecraft Server Wrapper",
            JavaExecutable = "java",
            ServerJar = "server.jar",
            JvmArguments = "-Xmx2G -Xms2G",
            Commands = new string[] { "say Server started 5 minutes ago", "time set day" } //default commands
        };

        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("config.json", configJson);
    }
}

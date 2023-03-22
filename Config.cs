using System.Text.Json;

public class Config
{
    public string ConsoleTitle { get; set; }
    public string JavaExecutable { get; set; }
    public string ServerJar { get; set; }
    public string JvmArguments { get; set; }

    // If config.json does not exist, creare it with default values
    public static void CreateDefaultConfig()
    {
        var config = new Config
        {
            ConsoleTitle = "Minecraft Server Wrapper",
            JavaExecutable = "java",
            ServerJar = "server.jar",
            JvmArguments = "-Xmx2G -Xms2G"
        };

        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("config.json", configJson);
    }
}

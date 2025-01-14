using Avans.StatisticalRobot;
using System.Reflection;
using System.Diagnostics;
using System.Text.Json;

// Startup class, to read and deserialize the appsettings.json
public class Startup
{
    public static string DbConnectionString { get; set; } = "";
    public Startup()
    {
        startup();
    }

    public static void startup()
    {
        Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());

        // Read and deserialize the appsettings.json
        string appSettingsPath = Directory.GetCurrentDirectory() + "/appsettings.json";
        var appSettings = DeserializeAppSettings(appSettingsPath);

        // Create SetConnection instance
        if (appSettings.MqttConnection == null || appSettings.MqttConnection.Host == null)
        {
            throw new InvalidOperationException("MqttConnection or Host is null in appsettings.json");
        }

        if (appSettings.ConnectionStrings == null || appSettings.ConnectionStrings.DefaultConnection == null)
        {
            throw new InvalidOperationException("ConnectionStrings or DefaultConnection is null in appsettings.json");
        }

        SetConnection connection = new SetConnection(
            appSettings.MqttConnection.Host,
            appSettings.ConnectionStrings.DefaultConnection
        );

        // Display version and build timestamp
        FileVersionInfo vi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        FileInfo fileInfo = new FileInfo(vi.FileName);
        DateTime createTime = fileInfo.CreationTime;
        Console.WriteLine($"SimpleRobot started (v{vi.FileVersion} @ {createTime}) ");
        Robot.PlayNotes("g>g");

        // Now you can use the connection strings from SetConnection
        // Console.WriteLine("MQTT Connection String: " + connection.GetMqttConnectionString());
        // Console.WriteLine("DB Connection String: " + connection.GetDbConnectionString());
        DbConnectionString = connection.GetDbConnectionString();
        Console.WriteLine("DEBUG: Startup finished");
    }

    public static AppSettings DeserializeAppSettings(string path)
    {
        string jsonString = File.ReadAllText(path);
        var appSettings = JsonSerializer.Deserialize<AppSettings>(jsonString);
        if (appSettings == null)
        {
            throw new InvalidOperationException("Failed to deserialize appsettings.json");
        }
        return appSettings;
    }
}
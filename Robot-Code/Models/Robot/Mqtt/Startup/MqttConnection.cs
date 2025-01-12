public class MqttConnection
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? ClientId { get; set; }
    public int TimeoutInMs { get; set; }
    public bool CleanStart { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

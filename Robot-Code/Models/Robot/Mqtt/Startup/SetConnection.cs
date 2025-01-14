
// startup, get mqtt and db connection strings from appsettings.json
public class SetConnection
{
    public string MqttConnectionString { get; set; }
    public string DbConnectionString { get; set; }

    public SetConnection(string mqttConnectionString, string dbConnectionString)
    {
        MqttConnectionString = mqttConnectionString;
        DbConnectionString = dbConnectionString;
    }

    public string GetMqttConnectionString()
    {
        return MqttConnectionString;
    }

    public string GetDbConnectionString()
    {
        return DbConnectionString;
    }

 
}
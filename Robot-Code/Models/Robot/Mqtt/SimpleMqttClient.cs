using HiveMQtt.Client;
using HiveMQtt.Client.Events;
using HiveMQtt.MQTT5.ReasonCodes;
using HiveMQtt.MQTT5.Types;
using System.Text;
using System.IO;
using System.Text.Json;

namespace SimpleMqtt;

public class SimpleMqttClient : IDisposable
{
    private static Encoding DefaultEncoding = Encoding.ASCII;
    private readonly HiveMQClient _client;
    public SimpleMqttClient(SimpleMqttClientConfiguration options)
    {
        this.ClientId = options.ClientId;

        _client = new HiveMQClient(new()
        {
            Host = options.Host!,
            ClientId = options.ClientId,
            Port = options.Port,
            ConnectTimeoutInMs = options.TimeoutInMs,
            UserName = options.UserName,
            Password = options.Password
        });

        _client.OnMessageReceived += OnHiveMQttMessageReceived;
    }

    public event EventHandler<SimpleMqttMessage>? OnMessageReceived;

    public string? ClientId { get; private set; }

    public async Task PublishMessage(SimpleMqttMessage message)
    {
        await this.OpenAndVerifyConnection();

        var mqttMessage = new MQTT5PublishMessage
        {
            Topic = message.Topic,
            Payload = DefaultEncoding.GetBytes(message.Message!),
            QoS = QualityOfService.ExactlyOnceDelivery,
        };

        var publishResult = await _client.PublishAsync(mqttMessage).ConfigureAwait(false);

        if (publishResult.QoS2ReasonCode != PubRecReasonCode.Success)
        {
            throw new InvalidOperationException($"Unable to publish message [reason code: {publishResult.QoS2ReasonCode.GetValueOrDefault(PubRecReasonCode.UnspecifiedError)}");
        }
    }

    public Task PublishMessage(string message, string topic) => PublishMessage(new() { Topic = topic, Message = message });

    public async Task SubscribeToTopic(string topic)
    {
        await this.OpenAndVerifyConnection();
        await _client.SubscribeAsync(topic, QualityOfService.ExactlyOnceDelivery).ConfigureAwait(false);
    }

    private void OnHiveMQttMessageReceived(object? sender, OnMessageReceivedEventArgs e)
    {
        // Trigger the new wrapped event with custom event arguments
        var msg = new SimpleMqttMessage
        {
            Topic = e.PublishMessage.Topic!,
            Message = DefaultEncoding.GetString(e.PublishMessage.Payload!)
        };

        this.OnMessageReceived?.Invoke(this, msg);
    }

    private async Task OpenAndVerifyConnection()
    {
        // Open de verbinding wanneer deze niet open is
        if (!this._client.IsConnected())
        {
            var connectionResult = await _client.ConnectAsync().ConfigureAwait(false);

            if (connectionResult.ReasonCode != ConnAckReasonCode.Success)
            {
                throw new InvalidOperationException($"Failed to connect: {connectionResult.ReasonString}");
            }
        }
    }

    public void Dispose() => _client.Dispose();

    ~SimpleMqttClient() => _client.Dispose();


    public static SimpleMqttClient CreateSimpleMqttClientForHiveMQ(string clientId)
    {
        // Define file path for easier debugging
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file not found at {filePath}");
        }

        // Attempt to read and deserialize the file as a JsonObject
        var json = File.ReadAllText(filePath);
        using var jsonDocument = JsonDocument.Parse(json);
        var jsonObject = jsonDocument.RootElement;

        // Extract properties from the nested "MqttConnection" object
        if (!jsonObject.TryGetProperty("MqttConnection", out var mqttConnection))
        {
            throw new InvalidOperationException("MqttConnection section not found in configuration.");
        }

        // Extract individual properties from the MqttConnection
        if (!mqttConnection.TryGetProperty("Host", out var host) || string.IsNullOrEmpty(host.GetString()))
        {
            throw new InvalidOperationException("Host not found in the MqttConnection configuration.");
        }

        var port = mqttConnection.GetProperty("Port").GetInt32();
        var clientIdConfig = mqttConnection.GetProperty("ClientId").GetString();
        var timeoutInMs = mqttConnection.GetProperty("TimeoutInMs").GetInt32();
        var userName = mqttConnection.GetProperty("UserName").GetString();
        var password = mqttConnection.GetProperty("Password").GetString();

        // Create and return the MQTT client
        return new SimpleMqttClient(new()
        {
            Host = host.GetString(),
            Port = port,
            ClientId = clientId,
            TimeoutInMs = timeoutInMs,
            UserName = userName,
            Password = password
        });
    }


}

public class SimpleMqttClientConfiguration
{

    public string? Host { get; set; }
    public int Port { get; set; }
    public string? ClientId { get; set; }
    public int TimeoutInMs { get; set; }
    public bool CleanStart { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
public class SimpleMqttMessage
{
    public string? Topic { get; set; }
    public string? Message { get; set; }
}

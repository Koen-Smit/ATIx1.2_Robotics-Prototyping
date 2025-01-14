using Avans.StatisticalRobot.Interfaces;
using HiveMQtt.MQTT5.Types;
using SimpleMqtt;

public class Communication : IUpdatable
{
    private readonly Battery _battery;
    // Define topics as constants
    private const string TopicDistance = "robot/distance";
    private const string TopicStatus = "robot/status";
    private const string TopicBattery = "robot/battery";
    public const string TopicTask = "robot/task";
    public const string TopicLux = "robot/lux";

    private readonly SimpleMqttClient _mqttClient;
    private readonly FZHRobot _robot;

    public Communication(FZHRobot robot)
    {
        _battery = new Battery();
        _robot = robot ?? throw new ArgumentNullException(nameof(robot));
        _mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ(GetClientId());
        _mqttClient.OnMessageReceived += OnMessageReceived;
    }

    // Initialize MQTT subscription
    public async Task Init()
    {
        // Subscribe to relevant topics
        await _mqttClient.SubscribeToTopic(TopicDistance);
        await _mqttClient.SubscribeToTopic(TopicStatus);
        await _mqttClient.SubscribeToTopic(TopicBattery);
        await _mqttClient.SubscribeToTopic(TopicTask);
        await _mqttClient.SubscribeToTopic(TopicLux);
        await _mqttClient.SubscribeToTopic("#");
        Console.WriteLine("Subscribed to topic: " + TopicStatus);

    }

    // Callback to handle incoming messages
    private void OnMessageReceived(object? sender, SimpleMqttMessage msg)
    {
        _robot.HandleMessage(msg);
    }

    // Generic method to publish messages to a given topic
    public async Task PublishMessageToTopic(string topic, string message)
    {
        if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Topic and message cannot be null or empty.");
        }

        await _mqttClient.PublishMessage(message, topic);
    }

    // Send battery measurement
    public async Task SendBatteryMeasurement(int _batteryPercentage)
    {
        await PublishMessageToTopic(TopicBattery, _batteryPercentage.ToString());
    }

    // Send tasktype
    public async Task SendTaskType(int number)
    {
        await PublishMessageToTopic(TopicTask, number.ToString());
    }

        // Send tasktype
    public async Task SendLux(int number)
    {
        await PublishMessageToTopic(TopicLux, number.ToString());
    }

    public async void Update()
    {
        await SendBatteryMeasurement(_battery.GetValue());
    }

    // Helper method to generate client ID
    private static string GetClientId() => $"{Environment.MachineName}-mqtt-client";
}

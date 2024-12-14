
using Avans.StatisticalRobot.Interfaces;
using HiveMQtt.MQTT5.Types;
using NLog.Layouts;
using SimpleMqtt;

public class CommunicationSystem : IUpdatable
{
    // Publish distance Distance to this topic
    const string topicDistance = "distance";

    private SimpleMqttClient mqttClient;
    private CaretakerRobot robot;
    private string clientId = $"{Environment.MachineName}-mqtt-client";

    public CommunicationSystem(CaretakerRobot robot)
    {
        this.robot = robot;
        mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ(clientId);
        mqttClient.OnMessageReceived += MessageCallback;
    }
    public async Task Init()
    {
        await mqttClient.SubscribeToTopic(topicDistance);
    }

    private void MessageCallback(object? sender, SimpleMqttMessage msg)
    {
        // Console.WriteLine($"DEBUG: MQTT message received: topic={msg.Topic}, msg={msg.Message}");
        robot.HandleMessage(msg);
    }

    public async Task SendDistanceMeasurement(int distance)
    {
        // Console.WriteLine($"DEBUG: Publishing distance measurement: topic={topicDistance}, msg={distance.ToString()}");
        await mqttClient.PublishMessage(distance.ToString(), topicDistance);
    }

    public void Update()
    {
        // Nothing
    }
}
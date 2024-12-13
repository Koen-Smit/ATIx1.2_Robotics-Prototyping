
using Avans.StatisticalRobot.Interfaces;
using HiveMQtt.MQTT5.Types;
using NLog.Layouts;
using SimpleMqtt;

public class CommunicationSystem : IUpdatable
{
    // Subscribe to this topic to receive commands
    const string topicCommand = "avansict/ict1.2/lesdemo/command";

    // Publish alert state on this topic
    const string topicAlert = "avansict/ict1.2/lesdemo/alert";

    // Publish distance measurements on this topic
    const string topicDistance = "avansict/ict1.2/lesdemo/distance";

    private SimpleMqttClient mqttClient;
    private WheeledRobot robot; // Store a reference to this so we can call its methods
    private string clientId = $"{Environment.MachineName}-mqtt-client";

    public CommunicationSystem(WheeledRobot robot)
    {
        this.robot = robot;
        mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ(clientId);
        // Add a callback method to the OnMessageReceived event
        // Note that we could have used a lambda expression here, with the actual
        // body of the method, but we'll use a named method here for clarity
        mqttClient.OnMessageReceived += MessageCallback;
    }

    /// <summary>
    /// Initializes the CommunicationSystem, e.g. subscribes to relevant MQTT topics
    /// </summary>
    /// <returns></returns>
    public async Task Init()
    {
        await mqttClient.SubscribeToTopic(topicCommand);
    }

    /// <summary>
    /// This is our internal callback method used to handle our incoming MQTT messages
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="msg">The message that was received</param>
    private void MessageCallback(object? sender, SimpleMqttMessage msg)
    {
        Console.WriteLine($"DEBUG: MQTT message received: topic={msg.Topic}, msg={msg.Message}");
        // Pass the message to the WheeledRobot so that it can handle it
        robot.HandleMessage(msg);
    }

    /// <summary>
    /// Publishes an MQTT message on the alert topic to reflect the new state
    /// </summary>
    /// <param name="state">String value representing the new state, e.g. "On" and "Off"</param>
    public async Task SendAlertState(string state)
    {
        Console.WriteLine($"DEBUG: Publishing alert state: topic={topicAlert}, msg={state}");
        await mqttClient.PublishMessage(state, topicAlert);
    }

    /// <summary>
    /// Publishes an MQTT message on the distance topic to show the most recently measured distance
    /// </summary>
    /// <param name="distance">The distance, in cm (published as a string value)</param>
    /// <returns></returns>
    public async Task SendDistanceMeasurement(int distance)
    {
        Console.WriteLine($"DEBUG: Publishing distance measurement: topic={topicDistance}, msg={distance.ToString()}");
        await mqttClient.PublishMessage(distance.ToString(), topicDistance);
    }

    public void Update()
    {
        // Nothing to do, MessageCallback will be called automatically when a message is received
    }
}
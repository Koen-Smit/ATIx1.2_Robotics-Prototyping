using SimpleMqtt;


public class StopService : IStop
{
    private readonly SimpleMqttClient _mqttClient;
    public string stopMessage { get; set; } = string.Empty;
    public string robotStatus { get; set; } = string.Empty;
    private readonly object _lock = new object();

    public StopService(SimpleMqttClient mqttClient)
    {
        _mqttClient = mqttClient;
    }

    // Start the robot
    public async Task StartRobot()
    {
        stopMessage = "started";
        await HandleStatus(stopMessage);
    }

    // Stop the robot
    public async Task StopRobot()
    {
        stopMessage = "stopped";
        await HandleStatus(stopMessage);
    }

    // Send an MQTT message to stop the robot, or start when it already stopped, topic: robot/status
    public async Task HandleStatus(string message)
    {
        // Ensure that only one action can happen at a time and avoid infinite loops
        await Task.Run(() =>
        {
            lock (_lock)
            {
                // Check if the robot is already stopped or running
                if (message == "stopped" && robotStatus != "Robot stopped.")
                {
                    _mqttClient.PublishMessage("stopped", "robot/status");
                    robotStatus = "Robot stopped.";
                }
                // Check if the robot is already running
                else if (message == "started" && robotStatus != "Robot running.")
                {
                    _mqttClient.PublishMessage("started", "robot/status");
                    robotStatus = "Robot running.";
                }
            }
        });
    }
}

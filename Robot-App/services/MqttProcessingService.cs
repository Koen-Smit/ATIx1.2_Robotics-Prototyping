using SimpleMqtt;

public class MqttProcessingService : IHostedService
{
    private readonly SimpleMqttClient _mqttClient;

    private readonly IBattery _batteryService;
    private readonly IStop _stopService; 
    private readonly ITask _taskService;

    public MqttProcessingService(SimpleMqttClient mqttClient, IBattery batteryRepository, IStop stop, ITask task)
    {
        _mqttClient = mqttClient;
        _batteryService = batteryRepository;
        _stopService = stop;
        _taskService = task;

        _mqttClient.OnMessageReceived += (sender, args) => {
            Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
            if (!string.IsNullOrEmpty(args.Message))
            {
                switch (args.Topic)
                {
                // Handle messages on the robot/battery topic
                case "robot/battery":
                    _batteryService.GetBatteryFromMqtt(args.Message);
                    break;

                // Handle messages on the robot/battery topic
                case "robot/status":
                    _stopService.HandleStatus(args.Message);
                    break;
                // Handle messages on the robot/task topic
                case "robot/task":
                    _taskService.GetTaskFromMqtt(args.Message);
                    break;
                default:
                    Console.WriteLine($"Unhandled topic: {args.Topic}");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Received empty or null message.");
            }
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Starting MQTT client...");
            await _mqttClient.SubscribeToTopic("#");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting MQTT client: {ex.Message}");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Stopping MQTT client...");
            _mqttClient.Dispose();
            Console.WriteLine("Disconnected from MQTT broker.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping MQTT client: {ex.Message}");
        }
        return Task.CompletedTask;
    }
}


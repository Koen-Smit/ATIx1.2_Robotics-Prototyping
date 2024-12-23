namespace Robot_App.Repositories
{
    using SimpleMqtt;
    public class MqttMessageProcessingService : IHostedService
    {
        private readonly IUserRepository _userRepository;
        private readonly SimpleMqttClient _mqttClient;

        public MqttMessageProcessingService(IUserRepository userRepository, SimpleMqttClient mqttClient)
        {
            _userRepository = userRepository;  
            _mqttClient = mqttClient;
            
            _mqttClient.OnMessageReceived += (sender, args) => {
                Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Starting MQTT client...");
                await _mqttClient.SubscribeToTopic("topic/#");
                Console.WriteLine("Subscribed to topic.");

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
}


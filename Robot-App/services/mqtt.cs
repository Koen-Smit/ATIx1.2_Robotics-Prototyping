using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Robot_App.Services
{
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;

        public MqttService()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
        }

        public async Task ConnectAsync(string host, int port, string clientId, string username, string password)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(host, port)
                .WithClientId(clientId)
                .WithCredentials(username, password)
                .WithCleanSession()
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to MQTT broker!");

                // Subscribe to a topic when connected
                var topic = "your/topic"; // Replace "your/topic" with your desired topic
                await _mqttClient.SubscribeAsync(new MQTTnet.Client.Subscribing.MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build());

                Console.WriteLine($"Subscribed to topic: {topic}");
            });

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Message received on topic {e.ApplicationMessage.Topic}: {message}");
                // Additional processing of the message can be done here
            });

            _mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("Disconnected from MQTT broker.");
                if (e.Exception != null)
                {
                    Console.WriteLine($"Disconnection reason: {e.Exception.Message}");
                }

                // Optional: Attempt to reconnect after a delay
                await Task.Delay(5000);
                try
                {
                    await _mqttClient.ReconnectAsync();
                    Console.WriteLine("Reconnected to MQTT broker.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnection failed: {ex.Message}");
                }
            });

            try
            {
                await _mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT connection failed: {ex.Message}");
            }
        }
    }
}

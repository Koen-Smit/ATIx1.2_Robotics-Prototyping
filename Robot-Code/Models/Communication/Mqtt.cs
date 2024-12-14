using SimpleMqtt;

namespace Models
{
    public class MqttUser
    {
        public async Task RunMqttClient()
        {
            var client = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("client-id");

            var receive = new MqttReceiver(client);
            receive.Receive();

            string topic = "TestMQtt";
            await client.SubscribeToTopic(topic);
            Console.WriteLine($"Subscribed to topic: {topic}\n");

            string text = string.Empty;
            var publish = new MqttPublisher(client)
            {
                Topic = topic,
                Message = text
            };

            Console.WriteLine("Your text:");
            text = Console.ReadLine() ?? string.Empty;
            publish.Message = text;

            publish.Publish();

            Console.WriteLine("Press any key to continue or 'q' to quit.");
            if (Console.ReadKey().KeyChar == 'q')
            {
                return;
            }
            Console.ReadKey();
        }
    }
}

using SimpleMqtt;

namespace Models
{
    public class MqttPublisher
    {
        public MqttPublisher(SimpleMqttClient client)
        {
            Client = client;
        }

        public string? Topic { get; set; }
        public string? Message { get; set; }
        public SimpleMqttClient Client { get; set; }

        public async void Publish()
        {
            if (Topic != null && Message != null)
            {
                await Client.PublishMessage(Message, Topic);
                // Console.WriteLine($"Message published; topic={Topic}; message={Message};\n");
                Console.WriteLine($"Message published.\n");
            }
            else
            {
                Console.WriteLine("Topic or Message is null.");
            }
        }
    }
}
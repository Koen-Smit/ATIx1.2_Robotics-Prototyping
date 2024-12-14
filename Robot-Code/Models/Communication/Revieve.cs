using SimpleMqtt;

namespace Models
{
    public class MqttReceiver
    {
        public MqttReceiver(SimpleMqttClient client)
        {
            Client = client;
        }
        public string? Topic { get; set; }
        public string? Message { get; set; }
        public SimpleMqttClient Client { get; set; }


        public void Receive()
        {
            // Subscribe to the OnMessageReceived event
            Client.OnMessageReceived += (sender, args) =>
            {
                Console.WriteLine($"Bericht ontvangen: topic={args.Topic}, message={args.Message}.\n");
            };
        }
    }
}
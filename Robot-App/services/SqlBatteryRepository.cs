
namespace Robot_App.Repositories
{
    using Microsoft.Data.SqlClient;
    using System.Collections.Generic;
    using SimpleMqtt;
    using HiveMQtt.Client;
    using HiveMQtt.Client.Events;
    using HiveMQtt.MQTT5.ReasonCodes;
    using HiveMQtt.MQTT5.Types;
    using System.Text;

    public class SqlBatteryRepository : IBatteryRepository
    {
        private readonly string _connectionString;
        private readonly HiveMQClient _client = new HiveMQClient(new());

        public SqlBatteryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void InsertBattery(Battery battery)
        {
            try
            {
                _client.OnMessageReceived += (sender, args) => {
                    Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
                    HandleBatteryMessage(args.Message);
                };
                // Insert battery percentage into the database
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [Battery] (Percentage) VALUES (@Percentage)"; 
                        command.Parameters.AddWithValue("@Percentage", battery.Percentage);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public List<Battery> GetBattery()
        {
            var batteryPercentages = new List<Battery>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Battery]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batteryPercentages.Add(new Battery
                            {
                                Percentage = reader.GetInt32(1)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return batteryPercentages;
        }

        public void HandleBatteryMessage(string message)
        {
            if (int.TryParse(message, out var batteryPercentage))
            {
                var battery = new Battery
                {
                    Percentage = batteryPercentage
                };

                InsertBattery(battery);
            }
            else
            {
                Console.WriteLine("Invalid battery percentage message received.");
            }
        }
        
        
        public class Battery
        {
            public int Percentage { get; set; }
        }
    }

    public interface IBatteryRepository
    {
        public void InsertBattery(SqlBatteryRepository.Battery battery);
        public List<SqlBatteryRepository.Battery> GetBattery();
        void HandleBatteryMessage(string message);
    }
}
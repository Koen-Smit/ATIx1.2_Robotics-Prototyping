using Microsoft.Data.SqlClient;
using SimpleMqtt;
public class BatteryService : IBattery
{
    private readonly SimpleMqttClient _mqttClient;
    private readonly string _connectionString;
    public List<Battery> batteries { get; set; }
    public BatteryService(string connectionString, SimpleMqttClient mqttClient)
    {
        _connectionString = connectionString;
        batteries = new List<Battery>();
        _mqttClient = mqttClient;

    }

    //get battery percentage from mqtt
    public void GetBatteryFromMqtt(string message)
    {
        InsertBattery(new Battery { Percentage = int.Parse(message), Timestamp = DateTime.Now });
    }

    // Insert battery into database
    public void InsertBattery(Battery battery)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [Battery] (Percentage) VALUES (@Percentage)";
                    command.Parameters.AddWithValue("@Percentage", battery.Percentage);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting battery: {ex.Message}");
        }
    }

    // Get battery from database
    public List<Battery> GetBattery()
    {
        var batteries = new List<Battery>();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ID, Percentage, Timestamp FROM [Battery]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batteries.Add(new Battery
                            {
                                Id = reader.GetInt32(0),
                                Percentage = reader.GetInt32(1),
                                Timestamp = reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching batteries: {ex.Message}");
        }
        return batteries;
    }

    // Delete battery from database
    public void DeleteBattery(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [Battery] WHERE ID = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting battery: {ex.Message}");
        }
    }

    // Load batteries for webpage
    public async Task LoadBatteries()
    {
        try
        {
            await _mqttClient.PublishMessage("update", "robot/status");

            // Wait for 500ms to ensure battery data is updated, then get battery data
            await Task.Delay(500);
            batteries = GetBattery();

            // If battery percentage is above 100, set it to 100
            foreach (var battery in batteries)
            {
                battery.Percentage = Math.Min(battery.Percentage, 100);
            }

            // Delete all batteries except the latest one
            while (batteries.Count > 1)
            {
                var oldestBattery = batteries.OrderBy(b => b.Timestamp).First();
                DeleteBattery(oldestBattery.Id);

                batteries = GetBattery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }

    // Battery color based on percentage for webpage
    public string GetBatteryColor(int percentage)
    {
        return percentage switch
        {
            >= 95 => "green",       // Fully green for 95% and above
            >= 80 => "darkgreen",   // Darker green for 80% to 94%
            >= 60 => "yellow",      // Yellow for 60% to 79%
            >= 40 => "orange",      // Orange for 40% to 59%
            >= 20 => "red",         // Red for 20% to 39%
            _ => "darkred",         // Dark red for 0% to 19%
        };
    }
}

using Microsoft.Data.SqlClient;
using SimpleMqtt;
public class LuxService : ILux
{
    private readonly SimpleMqttClient _mqttClient;
    private readonly string _connectionString;
    public List<Lux> luxes { get; set; }
    public LuxService(string connectionString, SimpleMqttClient mqttClient)
    {
        _connectionString = connectionString;
        luxes = new List<Lux>();
        _mqttClient = mqttClient;

    }

    //get battery percentage from mqtt
    public async void GetluxFromMqtt(string message)
    {
        await InsertLux(new Lux { Value = int.Parse(message), time_send = DateTime.Now });
    }

    //insert lux to database
    public async Task InsertLux(Lux lux)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [Lux] (Value, time_send) VALUES (@Value, @time_send)";

                    command.Parameters.AddWithValue("@Value", lux.Value);
                    command.Parameters.AddWithValue("@time_send", lux.time_send);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting lux: {ex.Message}");
        }
    }

    //load luxes from database
    public async Task LoadLuxes()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Lux]";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            luxes.Add(new Lux
                            {
                                Id = reader.GetInt32(0),
                                Value = reader.GetInt32(1),
                                time_send = reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading luxes: {ex.Message}");
        }
    }

    //get luxes
    public List<Lux> GetLuxes()
    {
        return luxes;
    }
}

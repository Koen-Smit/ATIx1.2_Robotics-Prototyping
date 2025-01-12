
using System.Text.Json;
public class PinReader
{
    private readonly Dictionary<string, object> _sensors;

    public PinReader(string configFilePath = "appsettings.json")
    {
        try
        {
            // Read the JSON content from the file
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/" + configFilePath);
            // Deserialize the JSON into a dictionary containing the 'Sensors' list
            var config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            // Access the 'Sensors' section and deserialize it into a list of Sensor objects
            var sensorsJson = config?["Sensors"].ToString();
            if (sensorsJson == null)
            {
                throw new Exception("Sensors section is missing in configuration.");
            }
            var sensors = JsonSerializer.Deserialize<List<Sensor>>(sensorsJson);

            // Initialize the sensors dictionary
            _sensors = new Dictionary<string, object>();
            if (sensors != null)
            {
                foreach (var sensor in sensors)
                {
                    if (sensor.SensorName != null)
                    {
                        if (sensor.Pin != null)
                        {
                            _sensors[sensor.SensorName] = sensor.Pin;
                        }
                        else
                        {
                            throw new Exception($"Pin for sensor '{sensor.SensorName}' is null in configuration.");
                        }
                    }
                    else
                    {
                        throw new Exception("Sensor name is null in configuration.");
                    }
                }
            }
            else
            {
                throw new Exception("Failed to deserialize sensors from configuration.");
            }

            Console.WriteLine("DEBUG: PinReader initialized successfully.");
        }
        catch (FileNotFoundException)
        {
            throw new Exception("appsettings.json file not found.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load sensor configuration: {ex.Message}");
        }
    }

    public int GetPin(string sensorName)
    {
        if (_sensors.ContainsKey(sensorName))
        {
            var pin = _sensors[sensorName].ToString();
            if (pin == null)
            {
                throw new Exception($"Pin for sensor '{sensorName}' is null.");
            }
            return pin.StartsWith("0x3E") ? -1 : Convert.ToInt32(pin);
        }
        else
        {
            throw new Exception($"Sensor '{sensorName}' not found in configuration.");
        }
    }

    public string GetPinString(string sensorName)
    {
        if (_sensors.ContainsKey(sensorName))
        {
            return _sensors[sensorName]?.ToString() ?? string.Empty;
        }
        else
        {
            throw new Exception($"Sensor '{sensorName}' not found in configuration.");
        }
    }
}

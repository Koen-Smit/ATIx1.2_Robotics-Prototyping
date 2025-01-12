using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;


// Lux class, used to get the light level
public class Lux : IUpdatable
{
    private readonly LightSensor lightSensor; // Light sensor for measurement
    private readonly PeriodTimer scanIntervalTimer; // Timer to control scanning frequency
    public int LuxLevel { get; private set; } // Light level

    private readonly PinReader pinReader; // PinReader instance

    public Lux(string sensorConfigPath = "appsettings.json", int scanIntervalMilliseconds = 500)
    {
        try
        {
            Console.WriteLine("DEBUG: Lux constructor called");

            // Initialize PinReader with the sensor configuration
            pinReader = new PinReader(sensorConfigPath);

            // Get the pin and initialize the ultrasonic sensor and scan timer
            int lightSensorPin = pinReader.GetPin("Light Sensor");
            lightSensor = new LightSensor((byte)lightSensorPin, scanIntervalMilliseconds);
            scanIntervalTimer = new PeriodTimer(scanIntervalMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to initialize LightDetection class. {ex.Message}");
            throw;
        }
    }


    public int GetValue()
    {
         if (scanIntervalTimer.Check())
        {
            LuxLevel = lightSensor.GetLux();
        }
        return LuxLevel;

    }

    public void Update()
    {
        GetValue();
    }
}

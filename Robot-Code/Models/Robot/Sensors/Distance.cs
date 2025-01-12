using Avans.StatisticalRobot.Interfaces;
using Avans.StatisticalRobot;

// Distance class, used to get the distance to the nearest obstacle
public class Distance : IUpdatable
{
    private PinReader pinReader; // PinReader object to read sensor configuration
    private readonly Ultrasonic _ultrasonicSensor; // Ultrasonic sensor for distance measurement
    private readonly PeriodTimer _scanIntervalTimer; // Timer to control scanning frequency
    public int ObstacleDistance { get; private set; } // Distance to the nearest obstacle

    public Distance(string sensorConfigPath = "appsettings.json", int scanIntervalMilliseconds = 500)
    {
        try
        {
            Console.WriteLine("DEBUG: Distance constructor called!");

            // Get the ultrasonic pin using PinReader
            pinReader = new PinReader(sensorConfigPath);
            int ultrasonicPin = pinReader.GetPin("Ultrasonic Sensor");

            // Initialize the ultrasonic sensor and scan timer
            _ultrasonicSensor = new Ultrasonic(ultrasonicPin);
            _scanIntervalTimer = new PeriodTimer(scanIntervalMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to initialize Distance class. {ex.Message}");
            throw;
        }
    }

    public int GetValue()
    {
        if (_scanIntervalTimer.Check())
        {
            return ObstacleDistance = _ultrasonicSensor.GetUltrasoneDistance();
        }
        
        return ObstacleDistance;
    }

    public void Update()
    {
        GetValue();
    }
}

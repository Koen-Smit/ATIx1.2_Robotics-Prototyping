using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class LightDetection : IUpdatable
{
    // Hardware related details
    const int LightSensorPinNumber = 0;
    const int ScanIntervalMilliseconds = 500;
    private LightSensor lightSensor; // Light measurement
    private PeriodTimer scanIntervalTimer;  // New distance measurement
    public int luxLevel { get; private set; } // Light level


    public LightDetection()
    {
        Console.WriteLine("DEBUG: LightDetection constructor called");
        lightSensor = new LightSensor(LightSensorPinNumber, ScanIntervalMilliseconds);
        scanIntervalTimer = new PeriodTimer(ScanIntervalMilliseconds);
        Update();
    }

    public void Update()
    {
        if (scanIntervalTimer.Check())
        {
            luxLevel = lightSensor.GetLux();;
        }
    }
}
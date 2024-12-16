using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class ObstacleDetectionSystem : IUpdatable
{
    // Hardware related details
    const int UltrasonicPinNumber = 18;
    const int ScanIntervalMilliseconds = 500;

    private Ultrasonic distanceSensor; // Distance measurement
    private PeriodTimer scanIntervalTimer;  // New distance measurement

    public int ObstacleDistance { get; private set; } // Distance to obstacle
    

    // Constructor for the ObstacleDetectionSystem class
    public ObstacleDetectionSystem()
    {
        Console.WriteLine("DEBUG: ObstacleDetectionSystem constructor called");
        distanceSensor = new Ultrasonic(UltrasonicPinNumber);
        scanIntervalTimer = new PeriodTimer(ScanIntervalMilliseconds);
        Update();
    }

    // Update the distance to the obstacle
    public void Update()
    {
        if (scanIntervalTimer.Check())
        {
            ObstacleDistance = distanceSensor.GetUltrasoneDistance();
        }
    }
}
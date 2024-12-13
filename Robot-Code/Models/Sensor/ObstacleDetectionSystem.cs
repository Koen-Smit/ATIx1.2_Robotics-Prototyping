using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class ObstacleDetectionSystem : IUpdatable
{
    // Define local constants for hardware related details
    const int UltrasonicPinNumber = 16; // GPIO pin for Ultrasonic sensor
    const int ScanIntervalMilliseconds = 500; // Optimize this for best responsiveness

    private Ultrasonic distanceSensor; // Used to perform the distance measurement
    private PeriodTimer scanIntervalTimer;  // Used to determine if a new distance measurement is due
    // Notes: PeriodicTimer automatically restarts itself when it has expired
    //        Call Check() to see if the PeriodicTimer has expired (returns true if it has)

    /// <summary>
    /// Contains the most recent obstacle distance measured, in cm
    /// </summary>
    public int ObstacleDistance { get; private set; }

    public ObstacleDetectionSystem()
    {
        Console.WriteLine("DEBUG: ObstacleDetectionSystem constructor called");
        distanceSensor = new Ultrasonic(UltrasonicPinNumber);
        scanIntervalTimer = new PeriodTimer(ScanIntervalMilliseconds);
    }

    /// <summary>
    /// Call this at regular intervals in order to detect obstacles
    /// </summary>
    public void Update()
    {
        // Don't measure at every call because it blocks all processing
        // during the measurement; so use a timer that times out periodically
        if (scanIntervalTimer.Check())
        {
            Robot.LEDs(0, 0, 255); // Flash the green LED on the Romi board
            ObstacleDistance = distanceSensor.GetUltrasoneDistance();
            Robot.LEDs(0, 0, 0);
        }
    }
}
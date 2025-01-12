using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

// Battery class, to get the battery percentage based on the total millivolts
public class Battery : IUpdatable
{
    private const int NumberOfBatteries = 6;
    private const int MinPerBattery = 100; // Minimum for each battery
    private const int MaxPerBattery = 4200; // Maximum for each battery

    public Battery()
    {
        Console.WriteLine("DEBUG: Battery constructor called");
    }

    // Get the current battery percentage based on the total millivolts
    public int CheckValue()
    {
        return Robot.ReadBatteryMillivolts(); // Get the total battery millivolts
    }

    // Calculate the battery percentage based on the millivolts/percentage
    private int Calculate(int totalMillivolts)
    {
        int minTotal = MinPerBattery * NumberOfBatteries;
        int maxTotal = MaxPerBattery * NumberOfBatteries;

        totalMillivolts = Math.Clamp(totalMillivolts, minTotal, maxTotal);

        // Calculate the percentage based on the min and max
        return (int)((totalMillivolts - minTotal) / (double)(maxTotal - minTotal) * 100);
    }

    public int GetValue()
    {
        return Calculate(CheckValue()); // Give the battery percentage
    }

    public void Update()
    {
        GetValue();
    }
}

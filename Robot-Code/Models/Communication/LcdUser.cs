using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class LcdUser : IUpdatable
{
    const int LcdAdress = 0x3E;
    private LCD16x2 lcdDisplay; // LCD display instance

    public LcdUser()
    {
        Console.WriteLine("DEBUG: LcdUser constructor called");
        lcdDisplay = new LCD16x2(LcdAdress);

        // Display "battery" on the LCD
        lcdDisplay.SetText("No battery measured yet");
        updateBatteryLevel();
    }

    // Update the battery level on the LCD
    public void updateBatteryLevel()
    {
        int totalMillivolts = Robot.ReadBatteryMillivolts();
        int batteryPercentage = CalculateBatteryPercentage(totalMillivolts);
        lcdDisplay.SetTextNoRefresh($"battery: {batteryPercentage} %");
    }

    // Calculate the battery percentage based on the millivolts/percentage
    private int CalculateBatteryPercentage(int totalMillivolts)
    {
        const int numberOfBatteries = 6;
        const int minVoltagePerBattery = 100;
        const int maxVoltagePerBattery = 4200;

        int minTotalVoltage = minVoltagePerBattery * numberOfBatteries;
        int maxTotalVoltage = maxVoltagePerBattery * numberOfBatteries;

        totalMillivolts = Math.Clamp(totalMillivolts, minTotalVoltage, maxTotalVoltage);
        
        return (int)((totalMillivolts - minTotalVoltage) / (double)(maxTotalVoltage - minTotalVoltage) * 100);
    }

    public void Update()
    {
        updateBatteryLevel();
    }
}
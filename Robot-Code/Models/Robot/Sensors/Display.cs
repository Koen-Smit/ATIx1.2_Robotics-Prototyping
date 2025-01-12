using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

// Display class, used to display information on the LCD display
public class Display : IUpdatable
{
    private LCD16x2 display;  // LCD display instance
    const int LcdAdress = 0x3E; // Pin for the LCD sensor

    public Display()
    {
        Console.WriteLine("DEBUG: Display constructor called");

        display = new LCD16x2(LcdAdress);
    }

    public void SetValue(string value)
    {
        display.SetText(value);
    }

    public void Update()
    {
        // Update the display
        // display.SetText("");
    }
}

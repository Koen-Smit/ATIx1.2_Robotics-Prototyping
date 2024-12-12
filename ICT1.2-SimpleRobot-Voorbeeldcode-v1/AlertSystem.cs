using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class AlertSystem : IUpdatable
{
    private Led alertLed;
    private Button emergencyButton;
    private bool emergencyButtonWasPressed;
    private LCD16x2 display;

    /// <summary>
    /// True if emergency stop button is pressed, false if it is not pressed
    /// </summary>
    public bool EmergencyStop { get; set; }

    public AlertSystem(Led led, Button button, LCD16x2 lcd)
    {
        Console.WriteLine("DEBUG: AlertSystem constructor called");

        display = lcd;
        emergencyButton = button;
        emergencyButtonWasPressed = emergencyButton.GetState().Equals("Pressed");
        alertLed = led;
        alertLed.SetOff();
    }

    /// <summary>
    /// Show an alert message and play an alert tune
    /// </summary>
    /// <param name="message">The message to be shown, one or two lines separated by \n</param>
    public void AlertOn(string message)
    {
        display.SetText(message);
        alertLed.SetOn();
        Robot.PlayNotes("fd");
    }

    /// <summary>
    /// Clear the alert message and play a non-alert tune
    /// </summary>
    public void AlertOff()
    {
        display.SetText(""); // Clear display
        alertLed.SetOff();
        Robot.PlayNotes("f>c");
    }

    /// <summary>
    /// Call this at regular intervals to ensure that the emergency button is scanned
    /// </summary>
    public void Update()
    {
        // Check if the emergency stop button state has changed and act accordingly
        if (emergencyButton.GetState() == "Pressed" && !emergencyButtonWasPressed)
        {
            Console.WriteLine("DEBUG: Emergency stop button pressed");
            emergencyButtonWasPressed = true;
            EmergencyStop = true;
        }
        else if (emergencyButton.GetState() == "Released" && emergencyButtonWasPressed)
        {
            Console.WriteLine("DEBUG: Emergency stop button released");
            emergencyButtonWasPressed = false;
            EmergencyStop = false;
        }
    }
}
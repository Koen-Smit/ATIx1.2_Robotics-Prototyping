using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class AlertSystem : IUpdatable
{
    private Button emergencyButton;
    private bool emergencyButtonWasPressed;
    public bool EmergencyStop { get; set; }

    public AlertSystem(Button button)
    {
        Console.WriteLine("DEBUG: AlertSystem constructor called");

        emergencyButton = button;
        emergencyButtonWasPressed = emergencyButton.GetState().Equals("Pressed");
    }

    public void AlertOn(string message)
    {
        Robot.PlayNotes("fd");
    }

    public void AlertOff()
    {
        Robot.PlayNotes("f>c");
    }
    public void Update()
    {
        // Check if the emergency stop button state has changed and act accordingly
        if (emergencyButton.GetState() == "Pressed" && !emergencyButtonWasPressed)
        {
            Console.WriteLine("DEBUG: Emergency stop button pressed");
            emergencyButtonWasPressed = true;
            EmergencyStop = true;

            // Stop the loop by throwing an exception or setting a flag
            throw new InvalidOperationException("Emergency stop activated! Halting execution.");
        }
        else if (emergencyButton.GetState() == "Released" && emergencyButtonWasPressed)
        {
            Console.WriteLine("DEBUG: Emergency stop button released");
            // Optionally allow resuming
            // emergencyButtonWasPressed = false;
            // EmergencyStop = false;
        }
    }
}
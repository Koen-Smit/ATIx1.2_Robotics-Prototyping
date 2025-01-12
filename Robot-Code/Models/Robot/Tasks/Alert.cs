using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

// AlertSystem class, to trigger an alert when needed
public class Alert : IUpdatable
{
    public void AlertOn()
    {
        Robot.PlayNotes("fd"); // Play sound when alert is triggered
    }

    public void AlertOff()
    {
        Robot.PlayNotes("f>c"); // Play sound when alert is turned off
    }

    public void Update()
    {
        // No additional logic required in Update for Alert
    }
}

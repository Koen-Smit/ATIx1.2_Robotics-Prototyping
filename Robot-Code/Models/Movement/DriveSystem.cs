using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class DriveSystem : IUpdatable
{
    private double speedStep = 0.05; // Default value

    public double SpeedStep {
        get
        {
            return speedStep;
        }

        set
        {
            // Ensure that SpeedStep stays within the range 0.0 to 1.0
            if (value > 0.0 && value <= 1.0)
            {
                speedStep = value;
            }
        }
    }

    private double targetSpeed = 0.0;

    public double TargetSpeed {
        get
        {
            return targetSpeed;
        }
        
        set
        {
            // Ensure that TargetSpeed remains within the range -1.0 to 1.0
            if (value >= -1.0 && value <= 1.0)
            {
                targetSpeed = value;
            }

        }
    }

    private double actualSpeed;

    public double ActualSpeed {
        get { return actualSpeed; }
    }

    public bool DriveActive { get; set; } = true;

    public DriveSystem()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
    }

    private short ToRobotSpeedValue(double speed)
    {
        return (short) Math.Round(speed * 300.0);
    }

     private void ControlRobotMotorSpeeds()
    {
        if (DriveActive)
        {
            // Set both motor speeds to the same value
            // because we only have one speed value
            Robot.Motors(
                ToRobotSpeedValue(actualSpeed),
                ToRobotSpeedValue(actualSpeed)
            );
        }
    }

    public void EmergencyStop()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
        ControlRobotMotorSpeeds();
    }

    public void Update()
    {
        if (actualSpeed < targetSpeed)
        {
            // Increase speed but don't exceed maximum of 1.0
            actualSpeed += speedStep;
            if (actualSpeed > 1.0)
            {
                actualSpeed = 1.0;
            }
            else if (actualSpeed > targetSpeed)
            {
                actualSpeed = targetSpeed;
            }
        }
        else if (actualSpeed > targetSpeed)
        {
            // Decrease speed but don't exceed minimum of -1.0
            actualSpeed -= speedStep;
            if (actualSpeed < -1.0)
            {
                actualSpeed = -1.0;
            }
            else if (actualSpeed < -targetSpeed)
            {
                actualSpeed = -targetSpeed;
            }
        }

        // Console.WriteLine($"DEBUG: Target speed {targetSpeed}, actual speed {actualSpeed}");
        
        // Send the new speed to the robot
        ControlRobotMotorSpeeds();
    }
}
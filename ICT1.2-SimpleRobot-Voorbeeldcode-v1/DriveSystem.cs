using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class DriveSystem : IUpdatable
{
    private double speedStep = 0.05; // Default value, can be changed at runtime

    /// <summary>
    /// Determines how quickly the actual speed reaches the target speed
    /// Must be between 0.0 and 1.0, where a lower value means slower acceleration
    /// </summary>
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

    /// <summary>
    /// Determines the speed for the robot
    /// Values: 0.0 = stop, -1.0 = full speed reverse, 1.0 = full speed forward
    /// </summary>
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

    /// <summary>
    /// The current speed, between -1.0 and 1.0, where 0.0 = stop
    /// Note that this property is read only, it cannot be set directly
    /// </summary>
    public double ActualSpeed {
        get { return actualSpeed; }
    }

    /// <summary>
    /// Controls whether the robot motors are actually engaged or not
    /// Set to false to prevent the robot from moving (useful while testing other features)
    /// </summary>
    public bool DriveActive { get; set; } = true;

    public DriveSystem()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
    }

    /// <summary>
    /// Converts the logical speed value (-1.0 to 1.0) to the value that
    /// the robot needs (-300 to 300)
    /// Note that 300 is approximately the maximum for the Romi robot motors
    /// </summary>
    /// <param name="speed">Logical speed value, should be between -1.0 and 1.0</param>
    /// <returns></returns>
    private short ToRobotSpeedValue(double speed)
    {
        return (short) Math.Round(speed * 300.0);
    }

    /// <summary>
    /// Sends the actual speed to both robot motors
    /// Note: we can currently only drive in a straight line (sort of)
    ///       because we have only one speed value and no turning information
    /// </summary>
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

    /// <summary>
    /// Immediately stops the robot and sets the target speed to zero
    /// </summary>
    public void EmergencyStop()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
        ControlRobotMotorSpeeds();
    }

    /// <summary>
    /// Call this at regular intervals to allow the DriveSystem to adjust
    /// the robot speed towards the target speed that is set
    /// </summary>
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

        Console.WriteLine($"DEBUG: Target speed {targetSpeed}, actual speed {actualSpeed}");
        
        // Send the new speed to the robot
        ControlRobotMotorSpeeds();
    }
}
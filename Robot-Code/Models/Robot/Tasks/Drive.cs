using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class Drive : IUpdatable
{
    private double speedStep = 0.05;
    private double targetSpeed = 0.0;
    private double actualSpeed = 0.0;
    public bool stopped = false;

    // Property for controlling speedStep with range check (0.0 to 1.0)
    public double SpeedStep
    {
        get => speedStep;
        set => speedStep = (value > 0.0 && value <= 1.0) ? value : speedStep;
    }

    // Property for controlling targetSpeed with range check (-1.0 to 1.0)
    public double TargetSpeed
    {
        get => targetSpeed;
        set => targetSpeed = (value >= -1.0 && value <= 1.0) ? value : targetSpeed;
    }

    // Read-only property for actualSpeed
    public double ActualSpeed => actualSpeed;

    // Flag for controlling whether the drive system is active
    public bool DriveActive { get; set; } = true;

    public Drive()
    {
        // Initialize speeds
        targetSpeed = 0.0;
        actualSpeed = 0.0;
    }

    // Converts the robot speed to a value the motors can understand
    private short ToRobotSpeedValue(double speed)
    {
        return (short)Math.Round(speed * 300.0);
    }

    // Controls the robot motor speeds based on the actual speed
    private void ControlRobotMotorSpeeds()
    {
        if (DriveActive)
        {
            // Set both motors to the same speed
            var motorSpeed = ToRobotSpeedValue(actualSpeed);
            Robot.Motors(motorSpeed, motorSpeed);
        }
    }

    // Emergency stop function, sets both speeds to 0
    public void EmergencyStop()
    {
        actualSpeed = targetSpeed = 0.0;
        ControlRobotMotorSpeeds();
    }

    // Update method to gradually change actualSpeed to targetSpeed
    public void Update()
    {
        // If the drive system is not active, set the target speed to 0
        if (!stopped && !DriveActive)
        {
            targetSpeed = 0.0;
        }
        // Adjust actualSpeed towards targetSpeed
        if (actualSpeed < targetSpeed)
        {
            actualSpeed = Math.Min(actualSpeed + speedStep, targetSpeed);
        }
        else if (actualSpeed > targetSpeed)
        {
            actualSpeed = Math.Max(actualSpeed - speedStep, targetSpeed);
        }

        // Ensure the motors are updated with the new actualSpeed
        ControlRobotMotorSpeeds();
    }
}

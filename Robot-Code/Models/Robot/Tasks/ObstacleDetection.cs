using Avans.StatisticalRobot;

public class ObstacleDetection
{
    private readonly Drive _drive;
    private readonly Distance _distance;
    public ObstacleDetection(Drive drive, Distance distance)
    {
        _drive = drive ?? throw new ArgumentNullException(nameof(drive));
        _distance = distance ?? throw new ArgumentNullException(nameof(distance));
    }

    // Updates the drive speed based on the obstacle distance
    private void SetDriveSpeedBasedOnDistance(int distance)
    {
        if (distance < 5)
        {
            // Stop the robot if obstacle is too close
            _drive.DriveActive = false;
            _drive.TargetSpeed = 0;
        }
        else if (distance >= 5 && distance < 15)
        {
            _drive.DriveActive = true;
            _drive.TargetSpeed = 0.1;
        }
        else if (distance >= 15 && distance < 40)
        {
            _drive.DriveActive = true;
            _drive.TargetSpeed = 0.2;
        }
        else if (distance >= 40)
        {
            _drive.DriveActive = true;
            _drive.TargetSpeed = 0.4;
        }
    }

    private async Task HandleObstacle(int distance)
    {
        // Stop the robot if obstacle is too close
        // Step 1: Stop
        _drive.DriveActive = false;
        _drive.TargetSpeed = 0;
        await Task.Delay(500); // Pause for half a second

        // Step 2: Move backward
        _drive.DriveActive = true;
        Robot.Motors(-100, -100);
        // await Task.Delay(1000); // Move backward for 1 second

        // Step 4: Turn
        Robot.Motors(-100, 100);
        await Task.Delay(1000);

        // Step 5: Stop after turning
        Robot.Motors(0, 0);
        await Task.Delay(500);

        // Step 6: Resume moving forward
        _drive.TargetSpeed = 0.2; // Set forward speed
    }


    // Update method that is called periodically to check and set drive speed
    public async void Update(int distance)
    {
        int obstacleDistance = distance;
        if (obstacleDistance < 8)
        {
            await HandleObstacle(obstacleDistance);
        }
        else
        {
            SetDriveSpeedBasedOnDistance(obstacleDistance);
        }
    }
}

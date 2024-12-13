using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;
using SimpleMqtt;

public class WheeledRobot : IUpdatable
{
    // Define local constants for hardware related details
    const int AlertLedPinNumber = 5;
    const int EmergencyStopButtonPinNumber = 6;
    const byte LcdAddress = 0x3E; // I2C address for the LCD grove module

    private DriveSystem driveSystem;
    private ObstacleDetectionSystem obstacleDetectionSystem;
    private AlertSystem alertSystem;
    private CommunicationSystem communicationSystem;
    private Led alertLed;
    private Button emergencyStopButton;
    private LCD16x2 lcd;
    private bool stopped = false;

    public WheeledRobot()
    {
        Console.WriteLine("DEBUG: WheeledRobot constructor called");

        // Create the DriveSystem and ObstacleDetectionSystem objects
        driveSystem = new DriveSystem();
        obstacleDetectionSystem = new ObstacleDetectionSystem();

        // Create the Led, Button and LCD16x2 objects for the AlertSystem to use
        alertLed = new Led(AlertLedPinNumber);
        emergencyStopButton = new Button(EmergencyStopButtonPinNumber);
        lcd = new LCD16x2(LcdAddress);

        // Pass references to the Led, the Button and the LCD16x2 to our new AlertSystem object
        alertSystem = new AlertSystem(alertLed, emergencyStopButton, lcd);

        // Create the CommunicationSystem object and give it a reference to our
        // own object (this) so that it can call our method HandleMessage()
        communicationSystem = new CommunicationSystem(this);
    }

    /// <summary>
    /// Initializes the WheeledRobot
    /// </summary>
    /// <returns></returns>
    public async Task Init()
    {
        Console.WriteLine("DEBUG: WheeledRobot Init() called");
        // Configure the DriveSystem so that it accelerates and decelerates slowly
        driveSystem.SpeedStep = 0.025;
        // Temporarily disable motors for quick testing during debugging
        driveSystem.DriveActive = false;

        // Configure the CommunicationSystem
        await communicationSystem.Init();
        await communicationSystem.SendAlertState("Off");
        
        // Let the user know that we're up and running
        lcd.SetText("SimpleRobot");
        Console.WriteLine("DEBUG: WheeledRobot Init() finished");
    }

    public void HandleMessage(SimpleMqttMessage msg)
    {
        Console.WriteLine($"Message received (topic:msg) = {msg.Topic}:{msg.Message}");
    }

    public async void Update()
    {
        // Call all logic components to let them do their internal updates
        obstacleDetectionSystem.Update();
        driveSystem.Update();
        alertSystem.Update();
        communicationSystem.Update();

        // Handle obstacle detection and influence on speed
        int distance = obstacleDetectionSystem.ObstacleDistance;
        await communicationSystem.SendDistanceMeasurement(distance);
        Console.WriteLine($"DEBUG: Distance {distance} cm");
        
        // Deal with any emergency stop conditions, such as
        // a nearby obstacle or the emergency button being pressed
        if ((distance < 3 && !stopped) || alertSystem.EmergencyStop)
        {
            stopped = true;
            driveSystem.EmergencyStop();
            alertSystem.AlertOn($"Emergency stop\nDistance {distance} cm");
            await communicationSystem.SendAlertState("On");
        }
        else if (distance >= 5 && stopped)
        {
            stopped = false;
            alertSystem.AlertOff();
            await communicationSystem.SendAlertState("Off");
        }

        // Determine the target speed based on the distance to the nearest obstacle
        if (distance >= 5 && distance < 15)
        {
            driveSystem.TargetSpeed = 0.1;
        }
        else if (distance >= 15 && distance < 40)
        {
            driveSystem.TargetSpeed = 0.2;
        }
        else if (distance >= 40)
        {
            driveSystem.TargetSpeed = 0.4;
        }
        
        Console.WriteLine($"DEBUG: Target speed {driveSystem.TargetSpeed}");
    }
}
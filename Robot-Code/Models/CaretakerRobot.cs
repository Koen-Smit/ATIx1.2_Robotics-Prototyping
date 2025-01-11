using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;
using SimpleMqtt;
using System.Reflection;
using System.Diagnostics;

public class CaretakerRobot : IUpdatable
{
    private DriveSystem driveSystem;
    private LcdUser lcdDisplay;
    private ObstacleDetectionSystem obstacleDetectionSystem;
    private CommunicationSystem communicationSystem;
    private AlertSystem alertSystem;
    private Button emergencyStopButton;
    const int EmergencyStopButtonPinNumber = 6;
    private bool stopped = false;

    public CaretakerRobot()
    {
        Console.WriteLine("DEBUG: CaretakerRobot constructor called");

        // Create the ObstacleDetectionSystem objects
        driveSystem = new DriveSystem();
        obstacleDetectionSystem = new ObstacleDetectionSystem();
        lcdDisplay = new LcdUser();

        emergencyStopButton = new Button(EmergencyStopButtonPinNumber);
        alertSystem = new AlertSystem(emergencyStopButton);

        communicationSystem = new CommunicationSystem(this);

    }
    
    public async Task Init()
    {
        driveSystem.SpeedStep = 0.025;
        driveSystem.DriveActive = false;

        // Configure the CommunicationSystem
        await communicationSystem.Init();
        
        Console.WriteLine("DEBUG: WheeledRobot Init() finished");
    }
    
    public void HandleMessage(SimpleMqttMessage msg)
    {
        Console.WriteLine($"Message received (topic:msg) = {msg.Topic}:{msg.Message}");
    }

    public async void Update()
    {
        // Call all components
        lcdDisplay.Update();
        obstacleDetectionSystem.Update();
        driveSystem.Update();
        alertSystem.Update();
        communicationSystem.Update();

        int distance = obstacleDetectionSystem.ObstacleDistance;
        await communicationSystem.SendDistanceMeasurement(distance);
        // Console.WriteLine($"DEBUG: Distance {distance} cm");
        
        if ((distance < 3 && !stopped) || alertSystem.EmergencyStop)
        {
            stopped = true;
            driveSystem.EmergencyStop();
        }
        else if (distance >= 5 && stopped)
        {
            stopped = false;
        }

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
        
        // Console.WriteLine($"DEBUG: Target speed {driveSystem.TargetSpeed}");
    }

    public void startup()
    {
        // display version and build timestamp
        FileVersionInfo vi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        FileInfo fileInfo = new FileInfo(vi.FileName);
        DateTime createTime = fileInfo.CreationTime;
        Console.WriteLine($"SimpleRobot started (v{vi.FileVersion} @ {createTime}) ");
        Robot.PlayNotes("g>g");
    }
}
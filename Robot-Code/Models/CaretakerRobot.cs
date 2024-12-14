using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;
using SimpleMqtt;
using System.Reflection;
using System.Diagnostics;

public class CaretakerRobot : IUpdatable
{
    private ObstacleDetectionSystem obstacleDetectionSystem;
    private CommunicationSystem communicationSystem;
    private bool stopped = false;

    public CaretakerRobot()
    {
        Console.WriteLine("DEBUG: CaretakerRobot constructor called");

        // Create the ObstacleDetectionSystem objects
        obstacleDetectionSystem = new ObstacleDetectionSystem();
        communicationSystem = new CommunicationSystem(this);

    }
    
    public async Task Init()
    {
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
        obstacleDetectionSystem.Update();
        communicationSystem.Update();

        // Handle obstacle detection
        int distance = obstacleDetectionSystem.ObstacleDistance;
        await communicationSystem.SendDistanceMeasurement(distance);
        // Console.WriteLine($"DEBUG: Distance {distance} cm");
    }

    public void startup()
    {
        // display version and build timestamp
        FileVersionInfo vi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        FileInfo fileInfo = new System.IO.FileInfo(vi.FileName);
        DateTime createTime = fileInfo.CreationTime;
        Console.WriteLine($"SimpleRobot started (v{vi.FileVersion} @ {createTime}) ");
        Robot.PlayNotes("g>g");
    }
}
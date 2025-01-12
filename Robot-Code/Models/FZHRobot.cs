using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;
using SimpleMqtt;
using System.Diagnostics;

public class FZHRobot : IUpdatable
{
    private readonly Startup _startup;
    private readonly Drive _drive;
    private readonly Display _display;
    private readonly Distance _distance;
    private readonly Alert _alert;
    private readonly ButtonSystem _button;
    private readonly ObstacleDetection _obstacleDetection;
    private readonly Communication _communication;
    bool mqttStop = false; // MQTT stop signal
    private bool mqttStopCount = false; // Robot standBy
    private bool standBy = true; // Robot standBy



    // Constructor to initialize the components
    public FZHRobot()
    {
        Console.WriteLine("DEBUG: FZHRobot constructor called");

        // Create components
        _startup = new Startup();
        _drive = new Drive();
        _distance = new Distance();
        _display = new Display();
        _alert = new Alert();
        _button = new ButtonSystem();
        _obstacleDetection = new ObstacleDetection(_drive, _distance);
        _communication = new Communication(this);

        Init().Wait();
    }
    
    // Initialize components asynchronously
    public async Task Init()
    {
        _drive.SpeedStep = 0.025;
        _drive.DriveActive = false;
        _drive.EmergencyStop();

        // Initialize communication system
        await _communication.Init();
        
        Console.WriteLine("DEBUG: FZHRobot Init() finished");
    }
    
    // Handle received MQTT messages
    public void HandleMessage(SimpleMqttMessage msg)
    {
        Console.WriteLine($"Message received (topic:msg) = {msg.Topic}:{msg.Message}");
        if (msg.Topic == "robot/status")
        {
            if (msg.Message == "stopped")
            {
                Console.WriteLine("Robot stopped");
                mqttStop = true;
            }
            else if (msg.Message == "started")
            {
                Console.WriteLine("Robot started");
                mqttStop = false;
            }
        }
    }

    // Update all components and handle logic
    public async void Update()
    {
        // if stop signal is received through mqtt, stop the robot
        if(mqttStop)
        {
            // Stop the robot
            _drive.EmergencyStop();

            // Update the display one time
            if (!mqttStopCount)
            {
                _display.SetValue("In behandeling");
                mqttStopCount = true;
                standBy = true;
            }
        }
        // if stop signal is not received, or start is recieved again, update the robot and start
        else
        {
            // reset variables
            mqttStopCount = false;

            // if red button is pressed, the robot will change standby state
            _button.Update();
            standBy = _button.redIsOn;

            // Have standby mode when starting robot, to prevent it from driving off
            if (standBy)
            {
                _drive.EmergencyStop();
                // every 20 sec show task

                
            }
            else
            {
                // Update all systems
                _distance.Update();
                _drive.Update();
                int distance = _distance.ObstacleDistance;
                
                // Update obstacle detection
                _obstacleDetection.Update(distance);
            }
        }
    }

}

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
    private readonly Lux _lux;
    private readonly ObstacleDetection _obstacleDetection;
    private readonly Communication _communication;
    private readonly TaskTypeList _taskTypeList;
    bool mqttStop = false; // MQTT stop signal
    private bool mqttStopCount = false; // Robot standBy
    private bool standBy = true; // Robot standBy
    private int _delay = 20; // Task delay in seconds

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
        _lux = new Lux();
        _obstacleDetection = new ObstacleDetection(_drive, _distance);
        _communication = new Communication(this);
        _taskTypeList = new TaskTypeList(_button, _display, _communication, _alert, _delay);

        Init().Wait();
    }
    
    // Initialize components asynchronously
    public async Task Init()
    {
        _drive.SpeedStep = 0.025;
        _drive.DriveActive = false;
        _drive.EmergencyStop();
        _display.SetValue(""); // Clear the display

        // Initialize communication system
        await _communication.Init();
        
        Console.WriteLine("DEBUG: FZHRobot Init() finished");
    }

    // Handle received MQTT messages
    public async void HandleMessage(SimpleMqttMessage msg)
    {
        Console.WriteLine($"Message received (topic:msg) = {msg.Topic}:{msg.Message}");
        if (msg.Topic == "robot/status")
        {
            if (msg.Message == "stopped")
            {
                Console.WriteLine("Robot stopped");
                _alert.AlertOff();
                mqttStop = true;
            }
            else if (msg.Message == "started")
            {
                Console.WriteLine("Robot started");
                _display.SetValue(""); // Clear the display
                _alert.AlertOn();
                mqttStop = false;
            }
            else if (msg.Message == "update")
            {
                Console.WriteLine("Robot battery send");
                // calculate lux average and send to mqtt
                int lux = _lux.GetValue();
                int average = 0;
                for (int i = 0; i < 10; i++)
                {
                    average += _lux.GetValue();
                    await Task.Delay(100);
                }
                await _communication.SendLux(average / 10);

                _communication.Update();
            }
        }
    }

    // Update all components and handle logic
    public void Update()
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
                _taskTypeList.Update();
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

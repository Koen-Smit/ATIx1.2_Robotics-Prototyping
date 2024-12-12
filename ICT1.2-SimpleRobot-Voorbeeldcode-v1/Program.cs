using System;
using System.Diagnostics;
using System.Reflection;
using System.Device.Gpio;
using System.Device.I2c;
using GyroscopeCompass;
using Avans.StatisticalRobot;
using GyroscopeCompass.GyroscopeCompass;

// display version and build timestamp
FileVersionInfo vi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
FileInfo fileInfo = new System.IO.FileInfo(vi.FileName);
DateTime createTime = fileInfo.CreationTime;
Console.WriteLine($"SimpleRobot started (v{vi.FileVersion} @ {createTime}) ");
Robot.PlayNotes("g>g");

// Create a boolean for toggling the yellow led on the Romi board
bool blinkLedOn = true;

// Create the WheeledRobot object and initialize it
WheeledRobot robot = new WheeledRobot();
await robot.Init();

// Enter the main infinite processing loop
while (true)
{
    robot.Update(); // Let robot perform its functions

    // Blink yellow led on Romi board to show activity
    blinkLedOn = !blinkLedOn;
    Robot.LEDs(0, (byte) (blinkLedOn ? 255 : 0), 0);

    Robot.Wait(200); // This wait time can be optimized for better response
}
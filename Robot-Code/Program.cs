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


// using System.Device.Gpio;
// using Avans.StatisticalRobot;

// Led led5 = new Led(5);
// led5.SetOff();

// Button button6 = new Button(6);

// bool isLedOn = false;

// while (true)
// {
//     if (button6.GetState() == "Pressed")
//     {
//         isLedOn = !isLedOn;
//         if (isLedOn)
//         {
//             led5.SetOn();
//         }
//         else
//         {
//             led5.SetOff();
//         }

//         Console.WriteLine("Het aantal millivolts :" + Robot.ReadBatteryMillivolts()); 
//         Robot.Wait(200); 

//         if (Robot.ReadBatteryMillivolts() < 3000)
//         {
//             Robot.LEDs(255, 0, 0);
//         }
//         else
//         {
//             Robot.LEDs(0, 255, 0);
//         }
//     }
// }
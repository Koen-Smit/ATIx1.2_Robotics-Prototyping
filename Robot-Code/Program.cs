using System;
using System.Device.Gpio;
using System.Device.I2c;
using GyroscopeCompass;
using Avans.StatisticalRobot;
using GyroscopeCompass.GyroscopeCompass;

//Start the robot
CaretakerRobot robot = new CaretakerRobot();
robot.startup();

// Keep the robot running
while (true)
{
    robot.Update(); // Update the robot

    Robot.Wait(200); // Wait
}
using System.Drawing;
using Avans.StatisticalRobot;

CaretakerRobot robot = new CaretakerRobot();
robot.startup();

RGBSensor rgbSensor = new RGBSensor();

while (true)
{
    robot.Update();
    Robot.Wait(200);
}
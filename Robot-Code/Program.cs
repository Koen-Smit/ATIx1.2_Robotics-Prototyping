using Avans.StatisticalRobot;

CaretakerRobot robot = new CaretakerRobot();
robot.startup();

while (true)
{
    robot.Update();
    Robot.Wait(200);
}
using Avans.StatisticalRobot;

FZHRobot robot = new FZHRobot();

while (true)
{
    robot.Update();
    Robot.Wait(200);
}
using System.Device.Gpio;
using Avans.StatisticalRobot;

Led led5 = new Led(5);
led5.SetOff();

Button button6 = new Button(6);

bool isLedOn = false;

while (true)
{
    if (button6.GetState() == "Pressed")
    {
        isLedOn = !isLedOn;
        if (isLedOn)
        {
            led5.SetOn();
        }
        else
        {
            led5.SetOff();
        }

        Console.WriteLine("Het aantal millivolts :" + Robot.ReadBatteryMillivolts()); 
        Robot.Wait(200); 

        if (Robot.ReadBatteryMillivolts() < 3000)
        {
            Robot.LEDs(255, 0, 0);
        }
        else
        {
            Robot.LEDs(0, 255, 0);
        }
    }
}
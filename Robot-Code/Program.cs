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



 // Play the Song of Storms sequence
    // Robot.PlayNotes("D5");
    // Robot.Wait(400); // Wait for 400ms

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("D5");
    // Robot.Wait(400);

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("D5");
    // Robot.Wait(400);

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("A5");
    // Robot.Wait(400);

    // Robot.PlayNotes("G5");
    // Robot.Wait(400);

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("G5");
    // Robot.Wait(400);

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("G5");
    // Robot.Wait(400);

    // Robot.PlayNotes("F5");
    // Robot.Wait(400);

    // Robot.PlayNotes("G5");
    // Robot.Wait(400);
    // Add a short pause at the end of the loop for musical effect
    // Robot.Wait(800);
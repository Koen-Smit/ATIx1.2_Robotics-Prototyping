using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;

public class ButtonSystem : IUpdatable
{
    private Button redButton;    // Red button instance
    private Button blueButton;   // Blue button instance
    private Led redLed;          // Red LED instance
    private Led blueLed;         // Blue LED instance
    private PinReader pinReader; // PinReader instance to read sensor configurations
    public bool redIsOn = true; // Boolean to check if the LED is on (ON by default)
    public bool blueIsOn = true; // Boolean to check if the LED is on (ON by default)
    public bool EmergencyStop { get; private set; } = false;  // Emergency stop flag
    private Alert _alert = new Alert();

    public ButtonSystem(string configFilePath = "appsettings.json")
    {
        Console.WriteLine("DEBUG: ButtonSystem constructor called");

        // Initialize the PinReader and fetch the LED pins
        pinReader = new PinReader(configFilePath);
        int redLedPin = pinReader.GetPin("Red-Led Sensor");
        int blueLedPin = pinReader.GetPin("Blue-Led Sensor");

        // Initialize the LEDs
        //RED LED, Turn the red LED on by default
        redLed = new Led(redLedPin);
        redLed.SetOn(); 
        redIsOn = true;
        //BLUE LED, Turn the blue LED on by default
        blueLed = new Led(blueLedPin);
        blueLed.SetOn();
        blueIsOn = true; 

        // Initialize the buttons (button pin is LED pin + 1)
        redButton = new Button(redLedPin + 1);
        blueButton = new Button(blueLedPin + 1);
    }

    // Get the state of the red button
    public string GetRedButtonState()
    {
        return redButton.GetState();
    }

    // Get the state of the blue button
    public string GetBlueButtonState()
    {
        return blueButton.GetState();
    }

    // Toggle function for LED// Toggle function for LED
    public void ToggleLed(Led led)
    {
        if (IsOn(led)) // Check LED state
        {
            led.SetOff(); // Turn it off if it's on
            if (led == redLed) redIsOn = false;
            if (led == blueLed) blueIsOn = false;
        }
        else
        {
            led.SetOn(); // Turn it on if it's off
            if (led == redLed) redIsOn = true;
            if (led == blueLed) blueIsOn = true;
        }
    }


    // IsOn function checks the state of the LED based on color
    public bool IsOn(Led led)
    {
        if (led == redLed)
        {
            return redIsOn; // Return the state of the red LED
        }
        if (led == blueLed)
        {
            return blueIsOn; // Return the state of the blue LED
        }
        return false; // Default return if no match
    }

    // Update the state when LED is turned on or off
    public void SetOn(Led led)
    {
        if (led == redLed)
        {
            redIsOn = true;
        }
        else if (led == blueLed)
        {
            blueIsOn = true;
        }
    }

    public void SetOff(Led led)
    {
        if (led == redLed)
        {
            redIsOn = false;
        }
        else if (led == blueLed)
        {
            blueIsOn = false;
        }
    }

    public void Update()
    {
        // Check if the red button is pressed and toggle the red LED accordingly
        if (GetRedButtonState() == "Pressed")
        {
            Console.WriteLine("DEBUG: Red button pressed!");
            _alert.AlertOff();
            ToggleLed(redLed);
        }

        // Check if the blue button is pressed and toggle the blue LED accordingly
        if (GetBlueButtonState() == "Pressed")
        {
            Console.WriteLine("DEBUG: Blue button pressed!");
            // ToggleLed(blueLed);
        }
    }
}

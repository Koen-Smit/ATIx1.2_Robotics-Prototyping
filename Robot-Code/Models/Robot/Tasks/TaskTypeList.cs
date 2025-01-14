using Avans.StatisticalRobot.Interfaces;
using Microsoft.Data.SqlClient;

// TaskList class, used to get a random task from a list of tasks
public class TaskTypeList : IUpdatable
{
    private List<string> taskList;
    private readonly ButtonSystem _button;
    private readonly Display _display;
    private readonly Communication _communication;
    private readonly Alert _alert;
    private bool isTaskDisplayed = false; // Taak weergegeven
    private bool taskInProgress = false; // Taak in progress
    private DateTime lastTaskTime; // Tijd van de laatste taakmelding
    private int number;
    private int _delay; // Delay in seconds

    public TaskTypeList(ButtonSystem button, Display display, Communication communication, Alert alert, int delay)
    {
        try
        {
            Console.WriteLine("DEBUG: Task constructor called");

            _button = button;
            _display = display;
            _communication = communication;
            _alert = alert;
            lastTaskTime = DateTime.Now;
            _delay = delay;
            
            taskList = new List<string>();

            //Get tasks
            List<TaskType> taskTypes = GetTaskTypes();
            taskList.AddRange(taskTypes.Select(t => t.Name).Where(name => name != null)!);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to initialize DisplaySystem. {ex.Message}");
            throw;
        }
    }


       // get Tasktypes from db
    public List<TaskType> GetTaskTypes()
    {
        List<TaskType> taskTypes = new List<TaskType>();
        using (SqlConnection connection = new SqlConnection(Startup.DbConnectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM TaskType", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taskTypes.Add(new TaskType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }
        }
        return taskTypes;
    }

    public string GetValue()
    {
        // Choose a random task from the list
        Random rand = new Random();
        int randomNumber = rand.Next(0, taskList.Count);
        return randomNumber + "_" + taskList[randomNumber];
    }

   // every 20 sec show task
    private async void TaskDisplay()
    {
        if (!taskInProgress)
        {
            DisplayTask();
        }
        await TaskCompleted();
    }

    // Display a new task on the display
    private void DisplayTask()
    {
        if ((DateTime.Now - lastTaskTime).TotalSeconds >= _delay)
        {
            // Console.WriteLine("DEBUG: Displaying new task.");

            // Split the value into number and text
            string value = GetValue();
            string[] parts = value.Split('_');
            number = int.Parse(parts[0]); // First part as number
            string text = parts[1]; 
            // Console.WriteLine("DEBUG: Task number: " + number + ", Task text: " + text);

            _display.SetValue(text);
            taskInProgress = true; // Pause until blue button is pressed
            _alert.AlertOn();
        }

    }

    // Check if the task is completed
    private async Task TaskCompleted()
    {
        if (taskInProgress && _button.GetBlueButtonState() == "Pressed")
        {
            // Console.WriteLine("DEBUG: Blue button pressed, task completed.");
            taskInProgress = false;
            
            //Send tasktype to MQTT
            await _communication.SendTaskType(number);

            lastTaskTime = DateTime.Now; // Reset the timer after task completion
            _alert.AlertOff();
            _display.SetValue(""); // Clear the display
        }
    }

    public void Update()
    {
        TaskDisplay();   
    }
}

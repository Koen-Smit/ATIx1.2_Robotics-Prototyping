using Avans.StatisticalRobot.Interfaces;

// TaskList class, used to get a random task from a list of tasks
public class TaskTypeList : IUpdatable
{
    private List<string> taskList;

    public TaskTypeList()
    {
        try
        {
            Console.WriteLine("DEBUG: Task constructor called");
            
            taskList = new List<string>();
            taskList.AddRange(new List<string>
            {
                "Neem je medicatie",
                "Meet je bloeddruk",
                "Goedemorgen, tijd voor ontbijt",
                "Goedemiddag, tijd voor lunch",
                "Goedenavond, tijd voor avondeten",
                "Eet een gezond tussendoortje",
                "Tijd om de benen te strekken!",
                "Tijd voor een kopje thee",
                "Tijd voor een glas water"
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to initialize DisplaySystem. {ex.Message}");
            throw;
        }
    }

    public string GetValue()
    {
        // Choose a random task from the list
        Random rand = new Random();
        int randomNumber = rand.Next(0, taskList.Count);
        return taskList[randomNumber];
    }

    public void Update()
    {
        GetValue();
    }
}

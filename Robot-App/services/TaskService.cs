using Microsoft.Data.SqlClient;

public class TaskService : ITask
{
    private readonly string _connectionString;
    public List<TaskList> tasks { get; set; }
    public TaskService(string connectionString)
    {
        _connectionString = connectionString;
        tasks = new List<TaskList>();
    }

    // Insert task into database
    public async Task InsertTaskList(TaskList taskList)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [TaskList] (Pressed_At, Status, TaskTypeID) VALUES (@Pressed_At, @Status, @TaskTypeID)";
                    
                    command.Parameters.AddWithValue("@Pressed_At", taskList.Pressed_At);
                    command.Parameters.AddWithValue("@Status", taskList.Status);
                    command.Parameters.AddWithValue("@TaskTypeID", taskList.TaskTypeID);
                    
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting task: {ex.Message}");
        }
    }

    // Insert tasktype into database
    public async Task InsertTaskType(TaskType taskType)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [TaskType] (TypeName, Description) VALUES (@TypeName, @Description)";
                    
                    command.Parameters.AddWithValue("@TypeName", taskType.TypeName);
                    command.Parameters.AddWithValue("@Description", taskType.Description);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting tasktype: {ex.Message}");
        }
    }

    // Get task from mqtt
    public void GetTaskFromMqtt(string message)
    {
        int taskTypeId = int.Parse(message);

        var taskList = new TaskList
        {
            Pressed_At = DateTime.Now,
            Status = true,
            TaskTypeID = taskTypeId
        };

        InsertTaskList(taskList).Wait();
    }

}

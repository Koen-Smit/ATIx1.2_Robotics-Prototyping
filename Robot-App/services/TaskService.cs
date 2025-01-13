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
                    command.CommandText = "INSERT INTO [TaskList] (PressedAt, Status, TaskTypeID) VALUES (@PressedAt, @Status, @TaskTypeID)";

                    command.Parameters.AddWithValue("@PressedAt", taskList.PressedAt);
                    command.Parameters.AddWithValue("@Status", taskList.Status);
                    command.Parameters.AddWithValue("@TaskTypeID", taskList.TaskTypeID + 1); // +1 because the robot-list is zero-based

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

    // load tasks from database
    public async Task LoadTasks()
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    t.[ID] AS TaskID,
                    t.[PressedAt],
                    t.[Status],
                    t.[TaskTypeID],
                    tt.[TypeName] AS TaskTypeName,
                    tt.[Description] AS TaskTypeDescription
                FROM 
                    [dbo].[TaskList] t
                JOIN 
                    [dbo].[TaskType] tt ON t.[TaskTypeID] = tt.[ID]
                ORDER BY 
                    CAST(t.[PressedAt] AS DATE) DESC, 
                    CAST(t.[PressedAt] AS TIME) DESC";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tasks.Add(new TaskList
                {
                    Id = reader.GetInt32(0),  // TaskID from index 0
                    PressedAt = reader.GetDateTime(1),  // PressedAt from index 1
                    Status = reader.GetBoolean(2),  // Status from index 2
                    TaskTypeID = reader.GetInt32(3),  // TaskTypeID from index 3
                    TaskType = new TaskType { TypeName = reader.GetString(4) },  // TypeName from index 4
                    TaskTypeName = reader.GetString(4),  // TaskTypeName from index 4
                    TaskTypeDescription = reader.GetString(5)  // TaskTypeDescription from index 5
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }

    // Get tasks
    public List<TaskList> GetTasks()
    {
        return tasks;
    }

    // Get task from mqtt
    public void GetTaskFromMqtt(string message)
    {
        int taskTypeId = int.Parse(message);

        var taskList = new TaskList
        {
            PressedAt = DateTime.Now,
            Status = true,
            TaskTypeID = taskTypeId
        };

        InsertTaskList(taskList).Wait();
    }

}

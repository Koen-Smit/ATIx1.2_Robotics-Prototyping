public interface ITask
{
    List<TaskList> tasks { get; set; }
    List<TaskType> taskTypes { get; set; }
    void GetTaskFromMqtt(string message);
    Task InsertTaskList(TaskList taskList);
    Task InsertTaskType(TaskType taskType);
    Task LoadTasks();
    List<TaskList> GetTasks();
    List<TaskType> GetTaskTypes();
    Task RemoveTaskType(int id);
}
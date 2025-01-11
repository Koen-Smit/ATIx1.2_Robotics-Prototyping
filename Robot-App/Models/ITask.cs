public interface ITask
{
    void GetTaskFromMqtt(string message);
    Task InsertTaskList(TaskList taskList);
    Task InsertTaskType(TaskType taskType);
}
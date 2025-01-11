public class TaskList
{
    public int Id { get; set; }
    public DateTime Pressed_At { get; set; }
    public bool Status { get; set; }

    // Foreign Key
    public int TaskTypeID { get; set; }

    // Navigatie naar TaskType
    public TaskType? TaskType { get; set; }
}

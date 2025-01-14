public class TaskList
{
    public int Id { get; set; }
    public DateTime PressedAt { get; set; }
    public bool Status { get; set; }

    // Foreign Key
    public int TaskTypeID { get; set; }

    // Navigatie naar TaskType
    public TaskType? TaskType { get; set; }
    public string? TaskTypeName { get; set; }
    public string? TaskTypeDescription { get; set; }

}

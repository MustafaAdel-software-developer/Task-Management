namespace TaskManagmentCore.Models.TaskModels;

public class TaskCategory
{
    public int CategoryId {get; set;}
    public string CategoryName {get; set;}
    public ICollection<TaskItem> Tasks {get; set;}
}
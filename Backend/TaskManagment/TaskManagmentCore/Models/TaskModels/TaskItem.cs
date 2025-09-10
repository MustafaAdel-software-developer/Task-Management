using TaskManagmentCore.Models.TaskModels.Enums;

namespace TaskManagmentCore.Models.TaskModels;

public class TaskItem
{
    public int TaskId { get; set; }
    
    public string Title {get; set;}
    
    public string? Description {get; set;}
    
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime DueTo {get; set;}

    public Priority Priority { get; set; } = Priority.Low;
    public Status TaskStatus { get; set; } = Status.Todo;
    
    public int ReporterId {get; set;}
    public User Reporter {get; set;}
    
    public int? AssigneeId {get; set;}
    public User? Assignee {get; set;}
    
    public int TaskCategoryId {get; set;}
    public TaskCategory TaskCategory {get; set;}

    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
}
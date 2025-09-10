namespace TaskManagmentCore.Models.TaskModels;

public class TaskComment
{
    public int CommentId {get; set;}
    
    public string CommentText {get; set;}
    public DateTime CommentDate {get; set;} = DateTime.UtcNow;
    
    public int TaskId { get; set; }
    public TaskItem TaskItem {get; set;}

    public int UserId { get; set; }
    public User User {get; set;}
}
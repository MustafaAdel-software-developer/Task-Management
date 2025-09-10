using TaskManagmentCore.Models.TaskModels;

namespace TaskManagmentCore.Models;

public class User
{
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string HashedPassword {get; set;}
   
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    
    public ICollection<TaskItem> ReportedTasks {get; set;} = new List<TaskItem>();
    
    public ICollection<TaskItem> AssignedTasks {get; set;} = new List<TaskItem>();
}
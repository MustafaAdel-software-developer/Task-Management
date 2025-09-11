using TaskManagmentCore.Models.TaskModels;

namespace TaskManagmentCore.Models;

public class User
{
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string HashedPassword {get; set;}

    public int RoleId { get; set; } = 2;
    public Role Role {get; set;}

    public string? RefreshToken {get; set;}
    
    public DateTime? RefreshTokenExpires {get; set;} 
    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    
    public ICollection<TaskItem> ReportedTasks {get; set;} = new List<TaskItem>();
    
    public ICollection<TaskItem> AssignedTasks {get; set;} = new List<TaskItem>();
}
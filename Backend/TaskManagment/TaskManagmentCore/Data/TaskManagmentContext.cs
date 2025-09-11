using Microsoft.EntityFrameworkCore;
using TaskManagmentCore.Models;
using TaskManagmentCore.Models.TaskModels;

namespace TaskManagmentCore;

public class TaskManagmentContext(DbContextOptions<TaskManagmentContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks {get; set;}
    public DbSet<TaskCategory> TaskCategories { get; set; }
    public DbSet<TaskComment> TaskComments {get; set;}
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasData(
                new Role{RoleId = 1, RoleName = "Admin"},
                new Role{RoleId = 2, RoleName = "Deafult"}
                );
        
        modelBuilder.Entity<TaskItem>()
            .HasKey(t => t.TaskId);

        modelBuilder.Entity<TaskCategory>()
            .HasKey(c => c.CategoryId);

        modelBuilder.Entity<TaskComment>()
            .HasKey(c => c.CommentId);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Role>()
            .HasKey(r => r.RoleId);

        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Reporter)
            .WithMany(u => u.ReportedTasks)
            .HasForeignKey(t => t.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.TaskCategory)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.TaskCategoryId);

        modelBuilder.Entity<TaskComment>()
            .Property(c => c.CommentText).HasMaxLength(250);
        modelBuilder.Entity<TaskComment>()
            .HasOne(c => c.TaskItem)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId);

        modelBuilder.Entity<TaskComment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

    }
}
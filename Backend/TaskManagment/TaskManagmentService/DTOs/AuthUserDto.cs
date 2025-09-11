namespace TaskManagmentService.DTOs;

public record AuthUserDto(
    int UserId, 
    string UserName, 
    string Email, 
    string RoleName
    );



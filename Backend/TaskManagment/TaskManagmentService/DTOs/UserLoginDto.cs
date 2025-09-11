using System.ComponentModel.DataAnnotations;

namespace TaskManagmentService.DTOs;

public record UserLoginDto(
    [Required]
    string Email,
    [Required]
    string Password
    );
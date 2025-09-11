using System.ComponentModel.DataAnnotations;

namespace TaskManagmentService.DTOs;

public record UserRegistraionDto(
    [Required]
    string UserName,
    [Required]
    string Email,
    [Required]
    string Password);
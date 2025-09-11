using System.ComponentModel.DataAnnotations;

namespace TaskManagmentService.DTOs;

public record RefreshTokenRequestDto(
    [Required] string Email,
    [Required] string RefreshToken
    );
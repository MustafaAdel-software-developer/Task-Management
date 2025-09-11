using System.ComponentModel.DataAnnotations;

namespace TaskManagmentService.DTOs;

public record TokensDto
(
    [Required] string AccessToken,
    [Required] string RefreshToken
    );
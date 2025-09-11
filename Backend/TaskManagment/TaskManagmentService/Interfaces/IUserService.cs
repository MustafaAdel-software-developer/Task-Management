using TaskManagmentService.DTOs;

namespace TaskManagmentService.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(UserRegistraionDto userRegistraionDto, CancellationToken cancellationToken = default);
    Task<TokensDto?> LoginUserWithEmailAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
}



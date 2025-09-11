using TaskManagmentCore.Models;

namespace TaskManagmentCore.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiresAtUtc, CancellationToken cancellationToken = default);
    Task<User?> ValidateRefreshToken(string email, string refreshToken,CancellationToken cancellationToken = default);

}
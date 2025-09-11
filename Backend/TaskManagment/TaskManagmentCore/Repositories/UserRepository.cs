using Microsoft.EntityFrameworkCore;
using TaskManagmentCore.Models;
using TaskManagmentCore.Repositories.Interfaces;

namespace TaskManagmentCore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagmentContext _dbContext;

    public UserRepository(TaskManagmentContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expiresAtUtc, CancellationToken cancellationToken = default)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = expiresAtUtc;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<User?> ValidateRefreshToken(string email, string refreshToken, CancellationToken cancellationToken = default)
    {
        User? user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpires < DateTime.UtcNow) return null;
        return user;
    }
}

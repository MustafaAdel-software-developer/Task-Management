using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagmentCore.Models;
using TaskManagmentService.DTOs;
using TaskManagmentService.Interfaces;
using TaskManagmentCore.Repositories.Interfaces;

namespace TaskManagmentService.UserService;

public class TokenService(IConfiguration configuration, IUserRepository userRepository) : ITokenService
{
    private string RefreshTokenGenerator()
    {
        var randomNumber = new byte[32];
        using var range = RandomNumberGenerator.Create();
        range.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string AccessTokenGenerator(AuthUserDto user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
            new Claim(ClaimTypes.Role,user.RoleName)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescription = new JwtSecurityToken(
            issuer: configuration["AppSettings:Issuer"],
            audience: configuration["AppSettings:Audience"],
            expires: DateTime.UtcNow.AddDays(1),
            claims: claims,
            signingCredentials: credential
            );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescription);

    }

    public async Task<TokensDto?> UpdateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.ValidateRefreshToken(refreshTokenRequestDto.Email, refreshTokenRequestDto.RefreshToken,cancellationToken);
        if (user is null) return null;

        var authUser = new AuthUserDto(user.UserId, user.UserName, user.Email, user.Role.RoleName);
        var newRefresh = await GenerateAndSaveRefreshToken(user, cancellationToken);
        var access = AccessTokenGenerator(authUser);
        return new TokensDto(access, newRefresh);
    }

    public async Task<TokensDto?> GenerateTokens(AuthUserDto user, CancellationToken cancellationToken = default)
    {
        var dbUser = await userRepository.GetUserByEmailAsync(user.Email, cancellationToken) ?? null;
        if (dbUser is null) return null;
        var refresh = await GenerateAndSaveRefreshToken(dbUser, cancellationToken);
        var access = AccessTokenGenerator(user);
        return new TokensDto(access, refresh);
    }

    private async Task<string> GenerateAndSaveRefreshToken(User user, CancellationToken cancellationToken)
    {
        string refreshToken = RefreshTokenGenerator();
        await userRepository.UpdateRefreshTokenAsync(user, refreshToken, DateTime.UtcNow.AddDays(7), cancellationToken);
        return refreshToken;
    }
}
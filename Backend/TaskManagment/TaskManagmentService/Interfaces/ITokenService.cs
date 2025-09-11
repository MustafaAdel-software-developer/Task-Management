using TaskManagmentCore.Models;
using TaskManagmentService.DTOs;

namespace TaskManagmentService.Interfaces;

public interface ITokenService
{
    Task<TokensDto?> GenerateTokens(AuthUserDto user, CancellationToken cancellationToken = default);
    Task<TokensDto?> UpdateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default);
}
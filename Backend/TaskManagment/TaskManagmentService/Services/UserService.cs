using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using TaskManagmentCore.Models;
using TaskManagmentCore.Repositories.Interfaces;
using TaskManagmentService.DTOs;
using TaskManagmentService.Interfaces;
using TaskManagmentService.Mappers;

namespace TaskManagmentService.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public UserService(IUserRepository userRepository,ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    //TODO:: need to check if user is null notify user to create account else Invalid credentials
    public async Task<TokensDto?> LoginUserWithEmailAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email, cancellationToken);
        bool isVerified = VerifyPassword(user, userLoginDto.Password);
        if (user is null || !isVerified) return null;
        return await _tokenService.GenerateTokens(UserMapper.ToAuthUserDto(user), cancellationToken);
    }
    
    public async Task<UserDto> RegisterUserAsync(UserRegistraionDto userRegistraionDto, CancellationToken cancellationToken = default)
    {
        var user = UserMapper.ToUserEntity(userRegistraionDto);
        
        var existing = await _userRepository.GetUserByEmailAsync(user.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("Email already registered");
        }
        
        user.HashedPassword = HashPassword(userRegistraionDto.Password, user);
        
        var created = await _userRepository.AddUserAsync(user, cancellationToken);
        return new UserDto(created.UserName, created.Email);
    }
    
    private static string HashPassword(string password,User user)
    {
       return new PasswordHasher<User>().HashPassword(user, password);
    }
    
    private static bool VerifyPassword(User user, string password)
    {
        return new PasswordHasher<User>().VerifyHashedPassword(user,user.HashedPassword,password) == PasswordVerificationResult.Success;
    }
}



using TaskManagmentCore.Models;
using TaskManagmentService.DTOs;

namespace TaskManagmentService.Mappers;

public static class UserMapper
{
    public static User ToUserEntity(UserRegistraionDto userRegistraionDto)
    {
        return new User()
        {
            UserName = userRegistraionDto.UserName,
            Email = userRegistraionDto.Email,
            HashedPassword = userRegistraionDto.Password
        };
    }

    public static AuthUserDto ToAuthUserDto(User user)
    {
        return new AuthUserDto(user.UserId, user.UserName, user.Email,user.Role.RoleName);
    }
    
}
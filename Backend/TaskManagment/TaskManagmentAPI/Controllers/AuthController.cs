using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentService.DTOs;
using TaskManagmentService.Interfaces;

namespace TaskManagmentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistraionDto userRegistraion, CancellationToken ct)
    {
        try
        {
            UserDto? registered = await _userService.RegisterUserAsync(userRegistraion, ct);
            return CreatedAtAction(nameof(Register), new { email = registered.Email }, registered);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    //TODO:: need to check if user is null notify user to create account else Invalid credentials at controller level
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLogin, CancellationToken ct)
    {
        TokensDto? tokenDto = await _userService.LoginUserWithEmailAsync(userLogin, ct);
        if (tokenDto is null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        return Ok(tokenDto);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> UpdateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken ct)
    {
        TokensDto? tokens = await _tokenService.UpdateRefreshToken(refreshTokenRequestDto, ct);
        if (tokens is null || tokens.AccessToken is null || tokens.RefreshToken is null)
            return Unauthorized("Invalid Credentials Email or RefreshToken");
        return Ok(tokens);
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedOnlyEndPoints()
    {
        return Ok("you are logged in");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("AdminPanel")]
    public IActionResult AdminPanel()
    {
        return Ok("you are admin");
    }
}



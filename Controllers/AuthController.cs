using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
  private readonly IAuthService _authService = authService;

  // POST: api/Auth/register
  [HttpPost("register")]
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    var user = await _authService.RegisterAsync(registerDto);

    if (user == null) {
      return BadRequest(new { message = "Username or email already exists" });
    }

    return CreatedAtAction(nameof(Register), user);
  }

  // POST: api/Auth/login
  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await _authService.LoginAsync(loginDto);

    if (user == null) {
      return Unauthorized(new { message = "Invalid email or password" });
    }

    return Ok(user);
  }
}
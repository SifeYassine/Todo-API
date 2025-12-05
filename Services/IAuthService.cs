using TodoApi.DTOs;

namespace TodoApi.Services;

public interface IAuthService
{
  Task<UserDto?> RegisterAsync(RegisterDto registerDto);
  Task<UserDto?> LoginAsync(LoginDto loginDto);
}

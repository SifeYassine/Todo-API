using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;

public interface IAuthService
{
  Task<UserDto?> RegisterAsync(RegisterDto registerDto);
  Task<UserDto?> LoginAsync(LoginDto loginDto);
  Task<AccessToken?> ValidateTokenAsync(string plainToken);
  Task<bool> RevokeTokenAsync(string plainToken);
}

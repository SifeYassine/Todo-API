using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;

public class AuthService(AppDbContext context, IMapper mapper, IConfiguration configuration) : IAuthService
{
  private readonly AppDbContext _context = context;
  private readonly IMapper _mapper = mapper;
  private readonly IConfiguration _configuration = configuration;

  public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
  {
    var isExistingUser = await _context.Users
    .AnyAsync(user => user.Email == registerDto.Email || user.Username == registerDto.Username);

    if (isExistingUser) {
      return null;
    }

    var user = new User
    {
      Username = registerDto.Username,
      Email = registerDto.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return _mapper.Map<UserDto>(user);
  }

  public async Task<UserDto?> LoginAsync(LoginDto loginDto)
  {
    var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == loginDto.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) {
      return null;
    }

    var userDto = _mapper.Map<UserDto>(user);
    userDto.Token = await GenerateAccessTokenAsync(user);

    return userDto;
  }

  private async Task<string> GenerateAccessTokenAsync(User user)
  {
    var bytes = RandomNumberGenerator.GetBytes(32);
    var plainToken = Convert.ToBase64String(bytes);
    var tokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(plainToken)));
    
    var accessToken = new AccessToken
    {
      TokenHash = tokenHash,
      Name = "Web Access Token",
      UserId = user.Id,
      CreatedAt = DateTime.UtcNow,
      ExpiresAt = DateTime.UtcNow.AddDays(7)
    };

    _context.AccessTokens.Add(accessToken);
    await _context.SaveChangesAsync();

    return plainToken;
  }

  public async Task<AccessToken?> ValidateTokenAsync(string plainToken)
  {
    var tokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(plainToken)));
    
    var accessToken = await _context.AccessTokens
    .Include(token => token.User)
    .FirstOrDefaultAsync(token => token.TokenHash == tokenHash);

    if (accessToken == null || accessToken.ExpiresAt < DateTime.UtcNow) {
      return null;
    }

    return accessToken;
  }

  public async Task<bool> RevokeTokenAsync(string plainToken)
  {
    var tokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(plainToken)));
    
    var accessToken = await _context.AccessTokens.FirstOrDefaultAsync(token => token.TokenHash == tokenHash);

    if (accessToken == null) {
      return false;
    }

    _context.AccessTokens.Remove(accessToken);
    await _context.SaveChangesAsync();

    return true;
  }
}
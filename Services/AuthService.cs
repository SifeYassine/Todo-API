using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    if (await _context.Users.AnyAsync(user => user.Email == registerDto.Email)) {
      return null;
    }

    if (await _context.Users.AnyAsync(user => user.Username == registerDto.Username)) {
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

    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
    {
      return null;
    }

    var userDto = _mapper.Map<UserDto>(user);
    userDto.Token = GenerateJwt(user);

    return userDto;
  }

  private string GenerateJwt(User user)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    var token = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddDays(2),
      signingCredentials: credentials
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
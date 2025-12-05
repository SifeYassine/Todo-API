using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class RegisterDto
{
  [Required]
  [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
  public string Username { get; set; } = string.Empty;

  [Required]
  [EmailAddress(ErrorMessage = "Invalid email address.")]
  [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
  public string Email { get; set; } = string.Empty;

  [Required]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
  public string Password { get; set; } = string.Empty;
}

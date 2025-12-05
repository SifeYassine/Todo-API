using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class LoginDto
{
  [Required]
  [EmailAddress(ErrorMessage = "Invalid email address.")]
  [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
  public string Email { get; set; } = string.Empty;

  [Required]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
  public string Password { get; set; } = string.Empty;
}
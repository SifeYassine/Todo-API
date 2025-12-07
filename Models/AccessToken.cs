namespace TodoApi.Models;

public class AccessToken
{
  public int Id { get; set; }
  public string TokenHash { get; set; } = string.Empty;
  public string? Name { get; set; }
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime ExpiresAt { get; set; }
}
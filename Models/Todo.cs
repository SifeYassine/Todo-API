namespace TodoApi.Models;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
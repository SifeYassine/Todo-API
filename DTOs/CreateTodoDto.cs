using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class CreateTodoDto
{
  [Required(ErrorMessage = "Title is required")]
  [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 50 characters")]
  public string Title { get; set; } = string.Empty;

  [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
  public string? Description { get; set; }
}
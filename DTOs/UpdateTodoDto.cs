using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UpdateTodoDto
{
  [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
  public string? Title { get; set; }

  [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
  public string? Description { get; set; }

  public bool? IsComplete { get; set; }
}
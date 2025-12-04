using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService(TodoContext context, IMapper mapper) : ITodoService
{
  private readonly TodoContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<List<TodoDto>> GetAllTodosAsync()
  {
    var todos = await _context.Todos.OrderByDescending(todo => todo.CreatedAt).ToListAsync();

    return _mapper.Map<List<TodoDto>>(todos);
  }

  public async Task<TodoDto?> GetTodoByIdAsync(int id)
  {
    var todo = await _context.Todos.FindAsync(id);

    if (todo == null) {
      return null;
    }

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto)
  {
    var todo = new Todo
    {
      Title = createDto.Title,
      Description = createDto.Description,
      IsComplete = false,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    _context.Todos.Add(todo);
    await _context.SaveChangesAsync();

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto)
  {
    var todo = await _context.Todos.FindAsync(id);

    if (todo == null) {
      return null;
    }

    _mapper.Map(updateDto, todo);
    todo.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<bool> DeleteTodoAsync(int id)
  {
    var todo = await _context.Todos.FindAsync(id);

    if (todo == null) {
      return false;
    }

    _context.Todos.Remove(todo);
    await _context.SaveChangesAsync();

    return true;
  }
}
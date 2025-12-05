using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService(AppDbContext context, IMapper mapper) : ITodoService
{
  private readonly AppDbContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<List<TodoDto>> GetAllTodosAsync(int userId)
  {
    var todos = await _context.Todos
    .Where(todo => todo.UserId == userId)
    .OrderByDescending(todo => todo.CreatedAt)
    .ToListAsync();

    return _mapper.Map<List<TodoDto>>(todos);
  }

  public async Task<TodoDto?> GetTodoByIdAsync(int id, int userId)
  {
    var todo = await _context.Todos
    .FirstOrDefaultAsync(todo => todo.Id == id && todo.UserId == userId);

    if (todo == null) {
      return null;
    }

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto, int userId)
  {
    var todo = new Todo
    {
      Title = createDto.Title,
      Description = createDto.Description,
      IsComplete = false,
      UserId = userId,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    _context.Todos.Add(todo);
    await _context.SaveChangesAsync();

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto, int userId)
  {
    var todo = await _context.Todos
    .FirstOrDefaultAsync(todo => todo.Id == id && todo.UserId == userId);

    if (todo == null) {
      return null;
    }

    _mapper.Map(updateDto, todo);
    todo.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return _mapper.Map<TodoDto>(todo);
  }

  public async Task<bool> DeleteTodoAsync(int id, int userId)
  {
    var todo = await _context.Todos
    .FirstOrDefaultAsync(todo => todo.Id == id && todo.UserId == userId);

    if (todo == null) {
      return false;
    }

    _context.Todos.Remove(todo);
    await _context.SaveChangesAsync();

    return true;
  }
}
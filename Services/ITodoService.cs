using TodoApi.DTOs;

namespace TodoApi.Services;

public interface ITodoService
{
  Task<List<TodoDto>> GetAllTodosAsync();
  Task<TodoDto?> GetTodoByIdAsync(int id);
  Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto);
  Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto);
  Task<bool> DeleteTodoAsync(int id);
}
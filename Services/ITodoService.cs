using TodoApi.DTOs;

namespace TodoApi.Services;

public interface ITodoService
{
  Task<List<TodoDto>> GetAllTodosAsync(int userId);
  Task<TodoDto?> GetTodoByIdAsync(int id, int userId);
  Task<TodoDto> CreateTodoAsync(CreateTodoDto createDto, int userId);
  Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto, int userId);
  Task<bool> DeleteTodoAsync(int id, int userId);
}
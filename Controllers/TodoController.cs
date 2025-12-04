using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController(ITodoService todoService) : ControllerBase
{
  private readonly ITodoService _todoService = todoService;

  // GET: api/Todos
  [HttpGet]
  public async Task<ActionResult<List<TodoDto>>> GetAll()
  {
    var todos = await _todoService.GetAllTodosAsync();
    return Ok(todos);
  }

  // GET: api/Todos/5
  [HttpGet("{id}")]
  public async Task<ActionResult<TodoDto>> GetById(int id)
  {
    var todo = await _todoService.GetTodoByIdAsync(id);

    if (todo == null) {
      return NotFound();
    }

    return Ok(todo);
  }

  // POST: api/Todos
  [HttpPost]
  public async Task<ActionResult<TodoDto>> Create(CreateTodoDto createDto)
  {
    var todo = await _todoService.CreateTodoAsync(createDto);
    return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
  }

  // PATCH: api/Todos/5
  [HttpPatch("{id}")]
  public async Task<IActionResult> Update(int id, UpdateTodoDto updateDto)
  {
    var todo = await _todoService.UpdateTodoAsync(id, updateDto);

    if (todo == null) {
        return NotFound();
    }

    return Ok(todo);
  }

  // DELETE: api/Todos/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var wasDeleted = await _todoService.DeleteTodoAsync(id);

    if (!wasDeleted) {
      return NotFound();
    }

    return NoContent();
  }
}
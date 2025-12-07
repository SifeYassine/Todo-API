using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoController(ITodoService todoService) : ControllerBase
{
  private readonly ITodoService _todoService = todoService;
  private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  // GET: api/Todos
  [HttpGet]
  public async Task<ActionResult<List<TodoDto>>> GetAll()
  {
    var todos = await _todoService.GetAllTodosAsync(UserId);
    return Ok(todos);
  }

  // GET: api/Todos/5
  [HttpGet("{id}")]
  public async Task<ActionResult<TodoDto>> GetById(int id)
  {
    var todo = await _todoService.GetTodoByIdAsync(id, UserId);

    if (todo == null) {
      return NotFound();
    }

    return Ok(todo);
  }

  // POST: api/Todos
  [HttpPost]
  public async Task<ActionResult<TodoDto>> Create(CreateTodoDto createDto)
  {
    var todo = await _todoService.CreateTodoAsync(createDto, UserId);
    return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
  }

  // PATCH: api/Todos/5
  [HttpPatch("{id}")]
  public async Task<IActionResult> Update(int id, UpdateTodoDto updateDto)
  {
    var todo = await _todoService.UpdateTodoAsync(id, updateDto, UserId);

    if (todo == null) {
      return NotFound();
    }

    return Ok(todo);
  }

  // DELETE: api/Todos/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var wasDeleted = await _todoService.DeleteTodoAsync(id, UserId);

    if (!wasDeleted) {
      return NotFound();
    }

    return NoContent();
  }
}
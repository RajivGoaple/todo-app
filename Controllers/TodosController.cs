using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>Get all todo items | Rajiv | V3</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_todoService.GetAll());
    }

    /// <summary>Get a todo item by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var todo = _todoService.GetById(id);
        return todo is null ? NotFound() : Ok(todo);
    }

    /// <summary>Create a new todo item</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required." });

        var todo = _todoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    /// <summary>Update an existing todo item</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required." });

        var todo = _todoService.Update(id, request);
        return todo is null ? NotFound() : Ok(todo);
    }

    /// <summary>Delete a todo item</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _todoService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}

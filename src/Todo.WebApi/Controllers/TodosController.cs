using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Core;
using Todo.Infrastructure.Data;
using Todo.WebApi.Requests;

namespace Todo.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    private readonly TodoDbContext _todoDbContext;

    public TodosController(TodoDbContext todoDbContext)
    {
        _todoDbContext = todoDbContext;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<TodoItem>> CreateTodo(CreateTodoRequest request)
    {
        var todoItem = new TodoItem(request.Title!);
        
        await _todoDbContext.AddAsync(todoItem);
        await _todoDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateTodo), new { id = todoItem.Id }, todoItem);
    }

    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteTodo(int id)
    {
        var todo = await _todoDbContext.TodoItems.FindAsync(id);

        if (todo is null)
        {
            return NotFound();
        }

        todo.Complete();

        _todoDbContext.Update(todo);
        await _todoDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> GetTodoById(int id)
    {
        var todo = await _todoDbContext.TodoItems.FindAsync(id);

        if (todo is null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos()
    {
        var todos = await _todoDbContext
            .TodoItems
            .ToListAsync();

        return Ok(todos);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _todoDbContext.TodoItems.FindAsync(id);

        if (todo is null)
        {
            return NotFound();
        }

        _todoDbContext.Remove(todo);
        await _todoDbContext.SaveChangesAsync();

        return Ok();
    }
}

using TodoApp.Models;

namespace TodoApp.Services;

public interface ITodoService
{
    IEnumerable<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem Create(CreateTodoRequest request);
    TodoItem? Update(int id, UpdateTodoRequest request);
    bool Delete(int id);
}

public class TodoService : ITodoService
{
    private readonly List<TodoItem> _todos = new();
    private int _nextId = 1;

    public IEnumerable<TodoItem> GetAll() => _todos.AsReadOnly();

    public TodoItem? GetById(int id) =>
        _todos.FirstOrDefault(t => t.Id == id);

    public TodoItem Create(CreateTodoRequest request)
    {
        var todo = new TodoItem
        {
            Id = _nextId++,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _todos.Add(todo);
        return todo;
    }

    public TodoItem? Update(int id, UpdateTodoRequest request)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return null;

        todo.Title = request.Title;
        todo.Description = request.Description;
        todo.IsCompleted = request.IsCompleted;
        todo.UpdatedAt = DateTime.UtcNow;
        return todo;
    }

    public bool Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return false;
        _todos.Remove(todo);
        return true;
    }
}

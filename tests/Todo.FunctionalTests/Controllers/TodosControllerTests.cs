using System.Net;
using System.Net.Http.Json;
using Todo.Core;
using Todo.WebApi.Requests;

namespace Todo.FunctionalTests.Controllers;

public class TodosControllerTests : IClassFixture<TodoApplicationFactory>, IAsyncLifetime
{
    private readonly TodoApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public TodosControllerTests(TodoApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTodo_WithValidRequest_ReturnsResponseWith201StatusCode()
    {
        var request = new CreateTodoRequest() 
        {
            Title = "My first task" 
        };

        var response = await _httpClient.PostAsJsonAsync("/todos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdTodo = await response.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(createdTodo);
        Assert.Equal("My first task", createdTodo.Title);
        Assert.False(createdTodo.IsDone);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync();
}

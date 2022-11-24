using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Infrastructure.Data;

namespace Todo.FunctionalTests;

public class TodoApplicationFactory : WebApplicationFactory<Program>
{
    public TodoDbContext CreateTodoDbContext()
    {
        var configuration = Services.GetRequiredService<IConfiguration>();

        var dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Todo"))
            .Options;

        return new TodoDbContext(dbContextOptions);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services => 
        {
            using var scope = services.BuildServiceProvider().CreateScope();

            using var todoDbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            
            todoDbContext.Database.EnsureDeleted();
            todoDbContext.Database.EnsureCreated();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        using var todoDbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        await todoDbContext.Database.EnsureDeletedAsync();
        await todoDbContext.Database.EnsureCreatedAsync();
    }
}

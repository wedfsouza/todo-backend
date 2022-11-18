using System.ComponentModel.DataAnnotations;

namespace Todo.WebApi.Requests;

public class CreateTodoRequest
{
    [Required]
    public string? Title { get; set; }
}

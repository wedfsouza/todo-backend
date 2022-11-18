namespace Todo.Core;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; private set; }

    public TodoItem(string title)
    {
        Title = title;
    }

    public void Complete()
    {
        IsDone = true;
    }
}

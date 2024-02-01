namespace BO;

public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Alias { get; init; }
    public TaskStatus Status { get; init; }
    public override string ToString() => this.ToStringProperty();
}

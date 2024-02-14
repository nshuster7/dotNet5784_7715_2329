namespace BO;

public class TaskInEmployee//Short view of a list
{
    public int Id { get; init; }
    public string? Alias { get; init; }
    public override string ToString() => this.ToStringProperty();
}

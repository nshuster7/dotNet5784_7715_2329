namespace BO;

public class EmployeeInTask
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public override string ToString() => this.ToStringProperty();
}

namespace DO;
/// <summary>
/// A dependency entity that connects two tasks
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">Previous assignment ID number</param>
public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
 )
{
    public Dependency() : this(0) { } // Empty constructor for stage 3
}

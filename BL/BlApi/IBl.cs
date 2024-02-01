
namespace BlApi;

public interface IBl
{
    public IEmployee Employee { get; }
    public ITask Task { get; }
    public ITaskInEmployee TaskInEmployee { get; }
    public ITaskInList TaskInList { get; }
    public IEmployeeInTask EmployeeInTask { get; }

}

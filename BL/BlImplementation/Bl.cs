namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEmployee Employee => new EmployeeImplementation();
    public ITask Task => new TaskImplementation();
}

namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEmployee Employee => new IEmployeeImplementation();
    public ITask Task => new ITaskImplementation();
}

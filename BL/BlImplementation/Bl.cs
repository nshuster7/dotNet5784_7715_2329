namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEmployee Employee => new EmployeeImplementation();
    public ITask Task => new TaskImplementation();
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

}
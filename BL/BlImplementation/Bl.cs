namespace BlImplementation;
using BlApi;
using Bllmplementation;

internal class Bl : IBl
{
    public IEmployee Employee => new EmployeeImplementation();
    public ITask Task => new TaskImplementation();
    public IUser User =>  new UserImplementation();

    
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

}
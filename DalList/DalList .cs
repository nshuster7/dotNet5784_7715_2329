namespace Dal;
using DalApi;
//using DO;
sealed public class DalList : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEmployee Employee => new EmployeeImplementation();

    public ITask Task => new TaskImplementation();
}
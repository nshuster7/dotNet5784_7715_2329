namespace Dal;
using DalApi;
using System;

//using DO;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public IDependency Dependency => new DependencyImplementation();

    public IEmployee Employee => new EmployeeImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startProjectDate { get { return DataSource.Config.StartProjectDate; }  set { DataSource.Config.StartProjectDate = value; } }

    public IUser User =>  new UserImplementation();
}
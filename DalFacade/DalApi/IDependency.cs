namespace DalApi;
using DO;
public interface IDependency : ICrud<Dependency> 
{
    /// <summary>
    /// check if task1 depends on task2
    /// </summary>
    /// <param name="t1">id of task 1</param>
    /// <param name="t2">id of task 2</param>
    /// <returns>return the dependeny if it exist or null</returns>
    public Dependency? Check(int t1, int t2);
}

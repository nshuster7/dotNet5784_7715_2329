namespace DalApi;
using DO;
public interface IDependency : ICrud<Dependency> 
{
    public Dependency? Check(int t1, int t2);
}

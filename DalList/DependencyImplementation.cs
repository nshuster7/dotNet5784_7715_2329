namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency dep)
    {
        int newNum = DataSource.Config.NextDependencyId;
        Dependency newDep = dep with { Id = newNum };
        DataSource.Dependencies.Add(newDep);
        return newNum;
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new Exception($"Dependency with ID={id} doe's NOT exists");
        }
        else
            DataSource.Dependencies.RemoveAt(id);
    }
    //return if task1 Depends On Task2
    public Dependency? Read(int IdTask1, int IdTask2)
    {
        return DataSource.Dependencies.Find(dep => dep.DependentTask == IdTask1 && dep.DependsOnTask == IdTask2);
    }

    public Dependency? Read(int IdDependence)
    {
        return DataSource.Dependencies.Find(dep => dep.Id == IdDependence);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency dep)
    { 
       if (Read(dep.Id) == null) 
        { 
            throw new Exception($"Dependency with ID={dep.Id} doe's NOT exists"); 
        }
        Delete(dep.Id);
        DataSource.Dependencies.Add(dep);
    }
}

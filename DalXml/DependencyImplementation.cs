﻿using DalApi;
using DO;

namespace Dal;

internal class DependencyImplementation: IDependency
{
    readonly string s_dependency_xml = "dependency";

    public Dependency? Check(int t1, int t2)
    {
        throw new NotImplementedException();
    }

    public int Create(Dependency item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
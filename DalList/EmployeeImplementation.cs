namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EmployeeImplementation : IEmployee
{
    public int Create(Employee m)
    {
        if (Read(m.Id) != null)
            throw new Exception();
        DataSource.Employees.Add(m);
        return m.Id;
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new Exception();
        else
            DataSource.Employees.RemoveAt(id);
    }

    public Employee? Read(int id)
    {
        return DataSource.Employees.Find(x => x.Id == id);
    }

    public List<Employee> ReadAll()
    {
        return new List<Employee>(DataSource.Employees);
    }

    public void Update(Employee m)
    {
        if (Read (m.Id) == null) 
            throw new Exception();
        Delete(m.Id);
        DataSource.Employees.Add(m);
    }
}

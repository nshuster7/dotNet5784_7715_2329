namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EmployeeImplementation : IEmployee
{
    public int Create(Employee emp)
    {
        if (Read(emp.Id) == null)
        {
            DataSource.Employees.Add(emp);
            return emp.Id;
        }

        throw new Exception($"Worker with ID={emp.Id} already exists");
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new Exception($"Worker with ID={id} doe's NOT exists");
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

    public void Update(Employee emp)
    {
        if (Read (emp.Id) == null) 
            throw new Exception($"Worker with ID={emp.Id} doe's NOT exists");
        Delete(emp.Id);
        DataSource.Employees.Add(emp);
    }
}

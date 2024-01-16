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

        throw new DalAlreadyExistsException($"Worker with ID={emp.Id} already exists");
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Worker with ID={id} doe's NOT exists");
        else
            DataSource.Employees.RemoveAt(id);
    }

    public Employee? Read(int id)
    {
        return DataSource.Employees.FirstOrDefault(x => x.Id == id);
    }
    public Employee? Read(Func<Employee, bool> filter)  //stage 2
    {
        return DataSource.Employees.FirstOrDefault(filter);
    }
    public List<Employee> ReadAll()
    {
        return new List<Employee>(DataSource.Employees);
    }
    public IEnumerable<Employee> ReadAll(Func<Employee, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Employees
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Employees
               select item;
    }

    public void Update(Employee emp)
    {
        if (Read (emp.Id) == null) 
            throw new DalDoesNotExistException($"Worker with ID={emp.Id} doe's NOT exists");
        Delete(emp.Id);
        DataSource.Employees.Add(emp);
    }
    
}

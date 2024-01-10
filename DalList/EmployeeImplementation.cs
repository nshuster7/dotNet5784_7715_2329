namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EmployeeImplementation : IEmployee
{
    public int Create(Employee emp)
    {
        if (Read(emp.Id) == null)
        {
            DataSource.Employees.Add(emp);
            return emp.Id;
        }

        throw new Exception();//!!
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new Exception();//!!
        else
            DataSource.Employees.RemoveAt(id);
    }

    public Employee? Read(int id)
    {
        return DataSource.Employees.Find(x => x.Id == id);
    }

    public List<Employee> ReadAll()//!!
    {
        List<Employee> newEmployees = new List<Employee>();
        ///////
        return newEmployees;
    }

    public void Update(Employee emp)
    {
        if (Read (emp.Id) == null) 
            throw new Exception();//!!
        Delete(emp.Id);
        DataSource.Employees.Add(emp);
    }
}

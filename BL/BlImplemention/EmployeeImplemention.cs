namespace BlImplemention;
using BlApi;
using DalApi;
using BO;
using System.Collections.Generic;

internal class EmployeeImplemention : BlApi.IEmployee
{
    private DalApi.IDal _dal = Factory.Get;
    public int Create(Employee employee)
    {
        
    }
   

    public void Delete(int id)
    {
        
    }

    public Employee? Read(int id)
    {
       
    }

    public IEnumerable<Employee> ReadAll()
    {

    }

    public void Update(Employee employee)
    {
     
    }
}

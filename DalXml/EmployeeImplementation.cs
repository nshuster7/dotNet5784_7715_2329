using DalApi;
using DO;

namespace Dal;

internal class EmployeeImplementation: IEmployee
{
    readonly string s_employees_xml = "employees";

    public int Create(Employee item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Employee? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Employee? Read(Func<Employee, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Employee?> ReadAll(Func<Employee, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Employee item)
    {
        throw new NotImplementedException();
    }
}

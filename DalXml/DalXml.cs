using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public class DalXml : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEmployee Employee => new EmployeeImplementation();

    public ITask Task =>  new TaskImplementation();
}

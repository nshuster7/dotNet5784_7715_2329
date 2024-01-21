using DalApi;
using DO;
using System.Data.Common;
using System.Linq;
namespace Dal;

internal class DependencyImplementation: IDependency
{
    readonly string s_dependency_xml = "dependency";

    public Dependency? Check(int IdTask1, int IdTask2)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(dep => dep.DependentTask == IdTask1 && dep.DependsOnTask == IdTask2);

    }

    public int Create(Dependency item)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        int nextId = XMLTools.GetAndIncreaseNextId("data-config", "nextDependencyId");
        Dependency copy = item with { Id = nextId };
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} does NOT exist");
        }
        else
            dependencies.RemoveAll(it => it.Id == id);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
    }

    public Dependency? Read(int id)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(dependencies => dependencies.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        if (filter != null)
        {
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        return from item in dependencies
               select item;
    }

    public void Update(Dependency item)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        if (dependencies.RemoveAll(it => it.Id == item.Id) == 0)
        {
            throw new DalDoesNotExistException($"Course with ID= {item.Id} does Not exist");
        }
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
    }
}

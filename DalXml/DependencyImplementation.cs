using DalApi;
using DO;
namespace Dal;
using System.Linq;
//using System.Data.Common;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependency_xml = "dependencies";
    /// <summary>
    /// Checks if a dependency exists between two tasks based on their IDs.
    /// </summary>
    /// <param name="IdTask1">The ID of the first task.</param>
    /// <param name="IdTask2">The ID of the second task.</param>
    /// <returns>The dependency object if found, null otherwise.</returns>
    public Dependency? Check(int IdTask1, int IdTask2)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(dep => dep.DependentTask == IdTask1 && dep.DependsOnTask == IdTask2);

    }

    /// <summary>
    /// Creates a new dependency and adds it to the dependency list.
    /// </summary>
    /// <param name="item">The dependency object to be created.</param>
    /// <returns>The ID of the newly created dependency.</returns>
    public int Create(Dependency item)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        int nextId = XMLTools.GetAndIncreaseNextId("data-config", "nextDependencyId");
        Dependency copy = item with { Id = nextId };
        dependencies.Add(copy);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
        return nextId;
    }

    /// <summary>
    /// Removes a dependency from the dependency list based on its ID.
    /// </summary>
    /// <param name="id">The ID of the dependency to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the task with the provided ID does not exist.</exception>
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

    /// <summary>
    /// Reads a dependency from the list based on its ID.
    /// </summary>
    /// <param name="id">The ID of the dependency to be read.</param>
    /// <returns>The dependency object if found, null otherwise.</returns>
    public Dependency? Read(int id)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(dependencies => dependencies.Id == id);
    }

    /// <summary>
    /// Reads the first dependency from the list that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that determines if a dependency matches the criteria.</param>
    /// <returns>The first dependency object that matches the filter, null if none found.</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<DO.Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependency_xml);
        return dependencies.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all dependencies from the list, optionally applying a filter.
    /// </summary>
    /// <param name="filter">An optional function that filters the dependencies.</param>
    /// <returns>An enumerable collection of dependency objects.</returns>
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

    /// <summary>
    /// Updates an existing dependency in the list.
    /// </summary>
    /// <param name="item">The updated dependency object.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the dependency with the provided ID does not exist.</exception>
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
    /// <summary>
    /// Empty the dependency file of data
    /// </summary>
    public void Clear()
    {
        // Load the dependencies from the XML file
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency_xml);

        // Clear the loaded list
        dependencies.Clear();

        // Save the empty list back to the XML file
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependency_xml);
    }
}

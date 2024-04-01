namespace Dal;
using DalApi;
using DO;
using System.Data.Common;

//using System.Data.Common;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// Empties the tasks file of data.
    /// </summary>
    public void Clear()
    {
        // Load the tasks from the XML file
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        // Clear the loaded list
        tasks.Clear();

        // Save the empty list back to the XML file
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        Config.NextTaskId = 1;
    }
    /// <summary>
    /// Creates a new task and adds it to the tasks XML file.
    /// </summary>
    /// <param name="item">The task object to be created.</param>
    /// <returns>The ID of the newly created task.</returns>
    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int nextId = XMLTools.GetAndIncreaseNextId("data-config", "nextTaskId");
        Task copy = item with { Id = nextId };
        tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        return nextId;
    }

    /// <summary>
    /// Deletes a task from the tasks XML file based on its ID.
    /// </summary>
    /// <param name="id">The ID of the task to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the task with the provided ID does not exist.</exception>
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} does NOT exist");
        }
        else
            tasks.RemoveAll(it => it.Id == id);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }

    /// <summary>
    /// Reads a task from the tasks XML file based on its ID.
    /// </summary>
    /// <param name="id">The ID of the task to be read.</param>
    /// <returns>The task object if found, null otherwise.</returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(task => task.Id == id);
    }

    /// <summary>
    /// Reads the first task from the tasks XML file that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that determines if a task matches the criteria.</param>
    /// <returns>The first task object that matches the filter, null if none found.</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all tasks from the tasks XML file, optionally applying a filter.
    /// </summary>
    /// <param name="filter">An optional function that filters the tasks.</param>
    /// <returns>An enumerable collection of task objects.</returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        // Apply the filter if provided, otherwise return all tasks
        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks
               select item;
    }

    /// <summary>
    /// Updates an existing task in the tasks XML file.
    /// </summary>
    /// <param name="item">The updated task object.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the task with the provided ID does not exist.</exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Tasks.RemoveAll(it => it.Id == item.Id) == 0)
        {
            throw new DalDoesNotExistException($"Task with ID= {item.Id} does Not exist");
        }
        Tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
    }
}

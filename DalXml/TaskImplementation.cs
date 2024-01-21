using DalApi;

namespace Dal;
using DO;
using System.Data.Common;

internal class TaskImplementation: ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        // Load tasks from XML
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        // Generate a unique ID
        int nextId = XMLTools.GetAndIncreaseNextId("task_config", "nextTaskId");
        Task copy= item with { Id = nextId };
        // Add the new task and save to XML
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        // Load tasks from XML
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        // Generate a unique ID
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} does NOT exist");
        }
        else
           // Add the new task and save to XML
           tasks.RemoveAll(it => it.Id ==id);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }

    public DO.Task? Read(int id)
    {
        // Load list from XML using XmlSerializer
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        // Find the task with the specified ID
        return tasks.FirstOrDefault(task => task.Id == id);
     }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        //  Load list from XML using XmlSerializer
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        //  Find the first task that matches the filter
        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        // Load list from XML using XmlSerializer
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

    public void Update(DO.Task item)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Tasks.RemoveAll(it => it.Id == item.Id) ==0) 
        {
            throw new DalDoesNotExistException($"Course with ID= {item.Id} does Not exist");
        }
        Tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
    }
}

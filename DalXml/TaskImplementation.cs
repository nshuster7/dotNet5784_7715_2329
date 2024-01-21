namespace Dal;
using DalApi;
using DO;
using System.Data.Common;

internal class TaskImplementation: ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int nextId = XMLTools.GetAndIncreaseNextId("data-config", "nextTaskId");
        Task copy= item with { Id = nextId };
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} does NOT exist");
        }
        else
           tasks.RemoveAll(it => it.Id ==id);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }

    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(task => task.Id == id);
     }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

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

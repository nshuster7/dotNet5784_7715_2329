namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newNum = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newNum };
        DataSource.Tasks.Add(newTask);
        return newNum;
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} doe's NOT exists");
        }
        else
            DataSource.Tasks.RemoveAt(id);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(x => x.Id == id);
    }
    public Task? Read(Func<Task, bool> filter)  //stage 2
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    public void Update(Task item)
    {
        if (Read(item.Id) == null)
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} doe's NOT exists");
        }
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}

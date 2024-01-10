namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        if (Read(item.Id) != null)
        {
            throw new Exception();
        }
        int newNum = DataSource.Config.NextTaskId;
        Task newTask = new Task(newNum);
        DataSource.Tasks.Add(newTask);
        return newNum;
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new Exception();
        }
        else
            DataSource.Tasks.RemoveAt(id);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(x => x.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) == null)
        {
            throw new Exception();
        }
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}

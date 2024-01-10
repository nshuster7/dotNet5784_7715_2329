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
        return DataSource.Tasks.Find()
    }

    public List<Task> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}

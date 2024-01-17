using DalApi;

namespace Dal;

internal class TaskImplementation: ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(DO.Task item)
    {
        throw new NotImplementedException();
    }
}

namespace BlApi;

public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null);
    public void Delete(int idTask);
    public void Clear();
}

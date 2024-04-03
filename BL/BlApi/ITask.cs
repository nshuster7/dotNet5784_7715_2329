using BO;

namespace BlApi;

public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null);
    public IEnumerable<BO.TaskInList> ReadAllTaskInList(Func<DO.Task, bool>? filter = null);
    public void Delete(int idTask);
    public void Update(BO.Task task);
    public bool CanTaskBeDeleted(DO.Task task);
    public bool IsTaskAvailable(DO.Task task);
    public BO.Type? GetComplexity(DO.Task task);
    public IEnumerable<BO.TaskInList>? TasksForWorker(int empId);
    public EmployeeInTask? GetEmployeeInTask(int idEmp);
    public void SignUpForTask(int idT, int idEmp);
    public void EndTask(int idT, int idEmp);
    public void StartTask(int idT, int idEmp);
    public List<DO.Dependency> FindDependencies(int id);
    public void Rec(List<BO.Task> tasks);
    public void AutomaticSchedule();
    public IEnumerable<IGrouping<BO.TaskStatus, DO.Task?>> GroupTasksByStatus();
    public bool InJeopardyCheck(int id);
}

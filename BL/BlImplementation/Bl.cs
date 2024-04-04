namespace BlImplementation;
using BlApi;
using Bllmplementation;
using BO;

internal class Bl : IBl
{
    public IEmployee Employee => new EmployeeImplementation(this);
    public ITask Task => new TaskImplementation(this);
    public IUser User =>  new UserImplementation(this);
    
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();
    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AdvanceTimeByYear()
    {
        Clock = Clock.AddYears(1);
    }

    public void AdvanceTimeByMonth()
    {
        Clock = Clock.AddMonths(1);
    }

    public void AdvanceTimeByDay()
    {
        Clock = Clock.AddDays(1);
    }

    public void AdvanceTimeByHour()
    {
        Clock = Clock.AddHours(1);
    }

    public void ResetTime()
    {
        Clock = DateTime.Now.Date;
    }

    public IEnumerable<Gant>? CreateGantList()
    {
        var lst = from item in Task.ReadAll()
                  let task = Task.Read(item.Id)
                  select new Gant
                  {
                      TaskId = task.Id,
                      TaskAlias = task.Alias,
                      EngineerId = task.Employee?.Id,
                      EngineerName = task.Employee?.Name,
                      StartDate = calculateStartDate(task.StartDate, task.ScheduledDate),
                      CompleteDate = calculateCompleteDate(task.ForecastDate, task.CompleteDate),
                      DependentTasks = lstDependentId(task.Dependencies),
                      Status = Tools.GetStatus(task)
                  };
        return lst.OrderBy(task => task.StartDate);

    }
    private IEnumerable<int> lstDependentId(IEnumerable<TaskInList>? dependencies)
    {
        return from dep in dependencies
               select dep.Id;
    }

    private DateTime calculateStartDate(DateTime? startDate, DateTime? scheduledDate)
    {
        if (startDate == null)
            return scheduledDate ?? Clock;
        else
            return startDate ?? Clock;
    }

    private DateTime calculateCompleteDate(DateTime? forecastDate, DateTime? completeDate)
    {
        if (completeDate == null)
            return forecastDate ?? Clock;
        else
            return completeDate ?? Clock;
    }
}

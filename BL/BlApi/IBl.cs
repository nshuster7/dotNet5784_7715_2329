using BO;

namespace BlApi;

public interface IBl
{
    public IEmployee Employee { get; }
    public ITask Task { get; }
    public IUser User { get; }

    private static DalApi.IDal dal = DalApi.Factory.Get;

    public static BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.IntermediateStage;
        if(dal.startProjectDate == null)
            projectStatus= BO.ProjectStatus.PlanningStage;
        if(dal.Task.ReadAll().FirstOrDefault(task=> task!.ScheduledDate!=null && task.StartDate!=null&& task.StartDate>dal.startProjectDate)!=null)
            projectStatus = BO.ProjectStatus.ExecutionStage;
        return projectStatus;
        
    }
    public static DateTime? ScheduleDateOffer(BO.Task task)
    {
        DateTime? dateTime = null;
        DO.Task _task;
        IEnumerable<DO.Dependency>? dependencies = (from dependency in dal.Dependency.ReadAll()
                                                    where dependency.DependentTask == task.Id
                                                    select dependency);
        if (dependencies == null && dal.startProjectDate!= null)
            return dal.startProjectDate;
        else
        {
            List<DO.Task>? tasksList = new();
            foreach (DO.Dependency dependency in dependencies!)
            {
                try
                {
                    DO.Task? doTask = dal.Task.Read(dependency.DependsOnTask??0);
                    if (doTask != null)
                        tasksList.Add(doTask);
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException($"Task with ID={dependency.DependsOnTask} doe's NOT exists", ex);
                }
            }
            if (tasksList.Any())
            {
                if (tasksList.FirstOrDefault(t => t.ScheduledDate == null) != null)
                    throw new BlScheduledDateException($"You cannot enter scheduled date for This with ID={task.Id} task");
                else
                {
                    _task = tasksList.MaxBy(t => Tools.GetMaxDate(t.StartDate, t.ScheduledDate)!.Value.Add(t.RequiredEffortTime ?? TimeSpan.Zero))!;
                    dateTime = Tools.GetMaxDate(_task.StartDate, _task.ScheduledDate)!.Value.Add(_task.RequiredEffortTime ?? TimeSpan.Zero);
                }
            }
        }
        return (DateTime)dateTime!;
    }

    /// <summary>
    /// Updating the schedule manually by request from the manager
    /// </summary>
    /// <exception cref="BlWrongValueException">Thrown when input with an invalid value is received</exception>
    public void ManualSchedule()
    {
        dal.startProjectDate = DateTime.Now;

        // Get all tasks
        IEnumerable<DO.Task?> tasks = dal.Task.ReadAll();

        // Get a list of tasks that do not depend on any other task
        IEnumerable<DO.Task?> independentTasks;
        if (tasks != null)
        {
            independentTasks = tasks.Where(t => t!=null && Tools.GetListOfPreviousTask(t.Id)!.Count()==0);

            // Schedule independent tasks
            foreach (DO.Task? task in independentTasks)
            {
                Console.WriteLine($"Enter the estimated start date for task with ID {task!.Id}");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime taskStartDate))
                    throw new BlWrongValueException("Invalid date");

                Tools.UpdateScheduledStartDate(task!.Id, taskStartDate);
                // Schedule dependent tasks recursively
                ScheduleTasks(tasks, task);
            }
        }
      
    }
   
    /// <summary>
    /// Schedules tasks recursively based on dependencies.
    /// </summary>
    /// <param name="tasks">The collection of tasks to be scheduled.</param>
    /// <param name="parentTask">The parent task on which dependent tasks are based.</param>
    /// <exception cref="BlWrongValueException">Thrown when an invalid value is encountered.</exception>
    private void ScheduleTasks(IEnumerable<DO.Task?> tasks, DO.Task? parentTask)
    {
        // Filter tasks that depend on the current task
        IEnumerable<DO.Task?> dependentTasks = tasks.Where(t => parentTask!=null && t != null && dal.Dependency.Check(t.Id, parentTask.Id) != null );

        // If there are dependent tasks
        if (dependentTasks.Any())
        {
            // Get start date for the first task
            Console.WriteLine($"Enter the estimated start date for task with ID {dependentTasks.First()!.Id}" +
                              "\nNote: This task may depend on other tasks" +
                              "\nand cannot be executed before the previous tasks are finished");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime taskStartDate))
                throw new BlWrongValueException("Invalid date");

            // Update estimated start date for the first task
            Tools.UpdateScheduledStartDate(dependentTasks.First()!.Id, taskStartDate);

            // Recursively schedule dependent tasks
            foreach (DO.Task? task in dependentTasks)
                ScheduleTasks(tasks, task);
        }
    }

    public void startProjectDate(DateTime date)
    {
        dal.startProjectDate= date;
    }
    public void InitializeDB();
    public void ResetDB();

    public DateTime Clock { get; }
    public void AdvanceTimeByYear();
    public void AdvanceTimeByDay();
    public void AdvanceTimeByMonth();   
    public void AdvanceTimeByHour();
    public void ResetTime();
    public IEnumerable<Gant>? CreateGantList();
  

}

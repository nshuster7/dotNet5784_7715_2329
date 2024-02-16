using BlImplementation;
using BO;
using DO;

namespace BlApi;

public interface IBl
{
    public IEmployee Employee { get; }
    public ITask Task { get; }

    private static DalApi.IDal dal = DalApi.Factory.Get;
    public static DateTime? startProjectDate { get; set; } = null;
    public static BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.IntermediateStage;
        if(startProjectDate == null)
            projectStatus= BO.ProjectStatus.PlanningStage;
        if(dal.Task.ReadAll().FirstOrDefault(task=> task!.ScheduledDate!=null && task.StartDate!=null&& task.StartDate<DateTime.Now)!=null)
            projectStatus = BO.ProjectStatus.ExecutionStage;
        return projectStatus;
    }
    //Ignore this function!!! it is a trying to write
    //public void AutomaticSchedule(DateTime projectStartDate)
    //{
    //    // Set the project start date in the configuration data
    //    IBl.startProjectDate = projectStartDate;

    //    // Retrieve all tasks
    //    IEnumerable<BO.Task> tasks = Task.ReadAll();

    //    // Iterate through each task to update the scheduled start date
    //    foreach (BO.Task task in tasks)
    //    {
    //        // Calculate the earliest start date for the task
    //        DateTime earliestStartDate = CalculateEarliestStartDate(task);

    //        // Update the scheduled start date for the task
    //        task.ScheduledDate = earliestStartDate;

    //        // Update the task in the database
    //        Task.Update(task);
    //    }
    //}
    //Ignore this function!!! it is a trying to write 
    //private DateTime CalculateEarliestStartDate(BO.Task task)
    //{
    //    // Retrieve dependencies for the task
    //    IEnumerable<BO.TaskInList> prevTasks = GetListOfPreviousTask(task.Id);

    //    // If the task has no dependencies, its scheduled start date should be the project start date
    //    if (!prevTasks.Any())
    //    {
    //        if (IBl.startProjectDate.HasValue)
    //        {
    //            return IBl.startProjectDate.Value;
    //        }
    //        else
    //        {
    //            throw new Exception("Project start date is not set.");
    //        }
    //    }

    //    // Calculate the latest forecasted end date among dependencies
    //    DateTime latestEndDate = prevTasks.Max(task =>
    //    {
    //        BO.Task? dependentTask = Task.Read(task.Id);
    //        return dependentTask.ForecastDate ?? DateTime.MinValue;
    //    });

    //    // If the project start date is not set or the latest end date among dependencies is later, return the latest end date
    //    if (!IBl.startProjectDate.HasValue || IBl.startProjectDate < latestEndDate)
    //    {
    //        return latestEndDate;
    //    }
    //    else
    //    {
    //        return IBl.startProjectDate.Value;
    //    }
    //}

    public void ManualSchedule()
    {
        IBl.startProjectDate = DateTime.Now;

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



}

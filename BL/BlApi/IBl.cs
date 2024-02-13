using BO;

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
    //public void ManualSchedule()
    //{
    //    Console.WriteLine("Enter the project start date");
    //    if (!DateTime.TryParse(Console.ReadLine(), out DateTime projectStartDate))
    //        throw new BlWrongValueException("WORNG DATE");
    //    IBl.startProjectDate = projectStartDate;
    //    dal.Task.ReadAll().Select(t=>updateScedualeDate(t));
    //}
    //public void updateScedualeDate(DO.Task task)
    //{

    //}

}

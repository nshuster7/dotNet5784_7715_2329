
namespace BlApi;

public interface IBl
{
    public IEmployee Employee { get; }
    public ITask Task { get; }

    private static DalApi.IDal dal = DalApi.Factory.Get;
    

    public static BO.ProjectStatus GetProjectStatus()
    {
        BO.ProjectStatus projectStatus = BO.ProjectStatus.IntermediateStage;
        if(dal.startProjectDate == null)
            projectStatus= BO.ProjectStatus.PlanningStage;
        if(dal.Task.ReadAll().FirstOrDefault(task=> task.ScheduledDate!=null && task.StartDate!=null&& task.StartDate<DateTime.Now)!=null)
            projectStatus = BO.ProjectStatus.ExecutionStage;
        return projectStatus;
    }


}

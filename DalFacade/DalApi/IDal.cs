namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IEmployee Employee { get; }
        ITask Task { get; }
        IUser User { get; }
        DateTime? startProjectDate { get; set; } 

    }
}

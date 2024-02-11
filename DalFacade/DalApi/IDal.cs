namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IEmployee Employee { get; }
        ITask Task { get; }
        public DateTime? startProjectDate { get; set; }
    }
}

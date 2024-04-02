namespace BO
{
    public class Gant
    {
        public int TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public BO.TaskStatus Status { get; set; }
        public int? EngineerId { get; set; }
        public string? TaskAlias { get; set; }
        public string? EngineerName { get; set; }
        public IEnumerable<int>? DependentTasks { get; set; }
    }
}

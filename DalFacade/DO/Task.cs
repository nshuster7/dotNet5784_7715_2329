namespace DO;

/// <summary>
/// 
/// </summary>
/// <param name="Id">A unique identifier for the task.</param>
/// <param name="EngineerId">The engineer assigned to the task.</param>
/// <param name="Alias">A short name for the task.</param>
/// <param name="Description">A detailed description of the task</param>
/// <param name="CreatedAtDate">The date and time the task was created.</param>
/// <param name="RequiredEffortTime">The estimated effort required to complete the task</param>
/// <param name="IsMilestone">Indicates whether the task is a milestone.</param>
/// <param name="Complexity">The complexity of the task</param>
/// <param name="StartDate">The start date for the task.</param>
/// <param name="ScheduledDate">The scheduled date for the task to be completed</param>
/// <param name="DeadlineDate">The latest possible date on which the completion of the task will not cause the project to fail.</param>
/// <param name="CompleteDate">The date and time the task was completed.</param>
/// <param name="Deliverables">A string describing the results or items provided at the end of the task</param>
/// <param name="Remarks">Additional remarks about the task.</param>
public record Task
(
    int Id,
    int EngineerId,
    string? Alias=null,
    string? Description = null,
    DateTime? CreatedAtDate=null,
    TimeSpan? RequiredEffortTime=null,
    bool IsMilestone=false,
    DO.Type? Complexity=null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate= null,
    DateTime? CompleteDate = null,
    string? Deliverables=null,
    string? Remarks=null
    
)
{
    public Task() : this(0, 0) { } // Empty constructor for stage 3
}

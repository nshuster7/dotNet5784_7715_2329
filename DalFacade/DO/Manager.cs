namespace DO;

/// <summary>
/// ShiftManager Entity represents a shift manager with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the manager (as in national id card)</param>
/// <param name="Name">Private Name of the manager</param>
/// <param name="Email">Email address of the shift manager</param>
/// <param name="Phone">Phone number of the shift manager</param>
/// <param name="IsOnShift">Indicates whether the shift manager is currently on shift</param>
/// <param name="ShiftStartTime">Start time of the current shift</param>
/// <param name="ShiftEndTime">End time of the current shift</param>
public record Manager
(
 int Id,
 string Name,
 string? Email = null,
 string? Phone = null,
 bool IsOnShift = false,
 DateTime? ShiftStartTime = null,
 DateTime? ShiftEndTime = null
)
{
    public Manager() : this(0, " ") { } // Empty constructor for stage 3
}
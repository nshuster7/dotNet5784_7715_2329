namespace DO;
public record Employee
(
    int Id,
    string Name,
    string Email,
    Specialization type,
    decimal HourlyRate,
    bool IsOnShift = false,
    DateTime? ShiftStartTime = null,
    DateTime? ShiftEndTime = null
)
{
    public Employee() : this(0, "", "" , 0) { } // Empty constructor for stage 3
}

public enum Specialization
{

}
using System.Xml.Linq;

namespace DO;
/// <summary>
/// Represents a construction worker with their essential details.
/// </summary>
/// <param name="Id">Unique identifier for the worker (e.g., employee ID).</param>
/// <param name="Name">Full name of the worker.</param>
/// <param name="Email">Email address of the worker.</param>
/// <param name="HourlyRate">Hourly payment rate of the worker.</param>
/// <param name="WorkStatus">Current work status of the worker (Active, Passive, or Terminated).</param>
/// <param name="Type">Type of construction work the worker specializes in (e.g., ConcreteWorker, Electrician, etc.).</param>

public record Employee
(
    int Id,
    string Name,
    string? Email = null,
    int HourlyRate = 0,
    WorkStatus? WorkStatus = null,
    Type? Type = null,
    string? ImageRelativeName =null
)
{
    public Employee() : this(0, " ") { } // Empty constructor for stage 3


}


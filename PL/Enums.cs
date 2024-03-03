using System.Collections;


namespace PL;
internal class EmployeeType : IEnumerable
{
    static readonly IEnumerable<BO.Type> s_enums =
(Enum.GetValues(typeof(BO.Type)) as IEnumerable<BO.Type>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class WorkStatus : IEnumerable
{
    static readonly IEnumerable<BO.WorkStatus> s_enums =
(Enum.GetValues(typeof(BO.WorkStatus)) as IEnumerable<BO.WorkStatus>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class TaskType : IEnumerable
{
    static readonly IEnumerable<BO.Type> s_enums =
(Enum.GetValues(typeof(BO.Type)) as IEnumerable<BO.Type>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
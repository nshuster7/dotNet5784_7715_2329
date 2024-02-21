using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


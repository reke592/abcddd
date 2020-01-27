using System.Collections.Generic;
using Payroll.Domain.Shared;

namespace Payroll.Domain.Employees
{
  public class EmployeeLeaveRequest : ValueObject
  {
    public string Start { get; private set; }
    public string Return { get; private set; }

    public static EmployeeLeaveRequest Create(Date start, Date end)
    {
      return new EmployeeLeaveRequest
      {
        Start = start.ToString(),
        Return = end.ToString()
      };
    }

    protected override IEnumerable<object> GetAtomicValues() {
      yield return Start;
      yield return Return;
    }
  }
}
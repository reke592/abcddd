using System;

namespace Payroll.Domain.Employees
{
  public class EmployeeId : AggregateId<EmployeeId>
  {
    public EmployeeId() {}
    public EmployeeId(Guid value)
    {
      Value = value;
    }

    public static implicit operator EmployeeId(Guid value) => new EmployeeId(value);
    public static implicit operator EmployeeId(string value) => new EmployeeId(Guid.Parse(value));
  }
}
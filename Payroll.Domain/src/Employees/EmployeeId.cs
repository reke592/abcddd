using System;

namespace Payroll.Domain.Employees
{
  public class EmployeeId : DomainAggregateGuid
  {
    public EmployeeId() {}
    public EmployeeId(Guid value) : base(value) { }

    public static implicit operator EmployeeId(Guid value) => new EmployeeId(value);
    public static implicit operator EmployeeId(string value) => new EmployeeId(Guid.Parse(value));

    // private Guid _value;

    // public EmployeeId(Guid value)
    // {
    //   if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
    //   _value = value;
    // }

    // public static implicit operator EmployeeId(Guid value) => new EmployeeId(value);
    // public static implicit operator Guid(EmployeeId self) => self._value;
    // public override string ToString() => _value.ToString();
  }
}
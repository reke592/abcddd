using System;

namespace Payroll.Domain.Employees
{
  public class EmployeeId
  {
    private Guid _value;

    public EmployeeId(Guid value)
    {
      _value = value;
    }

    public static implicit operator EmployeeId(Guid value) => new EmployeeId(value);
    public static implicit operator Guid(EmployeeId self) => self._value;
    public override string ToString() => _value.ToString();
  }
}
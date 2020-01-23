using System;

namespace Payroll.Domain.Salaries
{
  public class SalaryId
  {
    private Guid _value;

    public SalaryId(Guid value)
    {
      _value = value;
    }

    public static implicit operator SalaryId(Guid value) => new SalaryId(value);
    public static implicit operator Guid(SalaryId self) => self._value;
  }
}
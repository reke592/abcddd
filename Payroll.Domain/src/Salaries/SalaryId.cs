using System;

namespace Payroll.Domain.Salaries
{
  public class SalaryGradeId
  {
    private Guid _value;

    public SalaryGradeId(Guid value)
    {
      _value = value;
    }

    public static implicit operator SalaryGradeId(Guid value) => new SalaryGradeId(value);
    public static implicit operator Guid(SalaryGradeId self) => self._value;
  }
}
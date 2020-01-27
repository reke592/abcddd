using System;

namespace Payroll.Domain.PayrollPeriods
{
  public class PayrollPeriodId
  {
    private Guid _value;

    public PayrollPeriodId(Guid value)
    {
      _value = value;
    }

    public static implicit operator PayrollPeriodId(Guid value) => new PayrollPeriodId(value);
    public static implicit operator Guid(PayrollPeriodId self) => self._value;
    public override string ToString() => _value.ToString();
  }
}
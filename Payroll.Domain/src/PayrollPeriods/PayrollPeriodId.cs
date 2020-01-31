using System;

namespace Payroll.Domain.PayrollPeriods
{
  public class PayrollPeriodId : AggregateId<PayrollPeriodId>
  {

    public PayrollPeriodId() {}
    public PayrollPeriodId(Guid value)
    {
      Value = value;
    }

    public static implicit operator PayrollPeriodId(Guid value) => new PayrollPeriodId(value);
    public static implicit operator PayrollPeriodId(string value) => new PayrollPeriodId(Guid.Parse(value));
  }
}
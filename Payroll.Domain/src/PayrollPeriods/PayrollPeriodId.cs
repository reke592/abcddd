using System;

namespace Payroll.Domain.PayrollPeriods
{
  public class PayrollPeriodId : DomainAggregateGuid
  {

    public PayrollPeriodId() {}
    public PayrollPeriodId(Guid value) : base(value) { }

    public static implicit operator PayrollPeriodId(Guid value) => new PayrollPeriodId(value);
    public static implicit operator PayrollPeriodId(string value) => new PayrollPeriodId(Guid.Parse(value));
    // private Guid _value;

    // public PayrollPeriodId(Guid value)
    // {
    //   if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
    //   _value = value;
    // }

    // public static implicit operator PayrollPeriodId(Guid value) => new PayrollPeriodId(value);
    // public static implicit operator Guid(PayrollPeriodId self) => self._value;
    // public override string ToString() => _value.ToString();
  }
}
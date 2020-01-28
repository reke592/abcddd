using System;

namespace Payroll.Domain.Deductions
{
  public class DeductionId : DomainAggregateGuid
  {
    public DeductionId() {}
    public DeductionId(Guid value) : base(value) { }

    public static implicit operator DeductionId(Guid value) => new DeductionId(value);
    public static implicit operator DeductionId(string value) => new DeductionId(Guid.Parse(value));

    // private Guid _value;

    // public DeductionId(Guid value)
    // {
    //   if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
    //   _value = value;
    // }

    // public static implicit operator DeductionId(Guid value) => new DeductionId(value);
    // public static implicit operator Guid(DeductionId self) => self._value;
    // public override string ToString() => _value.ToString();
  }
}
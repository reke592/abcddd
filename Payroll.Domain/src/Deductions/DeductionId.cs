using System;

namespace Payroll.Domain.Deductions
{
  public class DeductionId : AggregateId<DeductionId>
  {
    public DeductionId() {}
    public DeductionId(Guid value)
    {
      Value = value;
    }

    public static implicit operator DeductionId(Guid value) => new DeductionId(value);
    public static implicit operator DeductionId(string value) => new DeductionId(Guid.Parse(value));
  }
}
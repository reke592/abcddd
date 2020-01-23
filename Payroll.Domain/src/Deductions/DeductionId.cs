using System;

namespace Payroll.Domain.Deductions
{
  public class DeductionId
  {
    private Guid _value;
    
    public DeductionId(Guid value)
    {
      _value = value;
    }

    public static implicit operator DeductionId(Guid value) => new DeductionId(value);
    public static implicit operator Guid(DeductionId self) => self._value;
  }
}
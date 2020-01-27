using System;

namespace Payroll.Domain.BusinessYears
{
  public class BusinessYearId
  {
    private Guid _value;
    
    public BusinessYearId(Guid value)
    {
      _value = value;
    }

    public static implicit operator BusinessYearId(Guid value) => new BusinessYearId(value);
    public static implicit operator Guid(BusinessYearId self) => self._value;
    public override string ToString() => _value.ToString();
  }
}
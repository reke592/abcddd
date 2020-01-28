using System;

namespace Payroll.Domain.BusinessYears
{
  public class BusinessYearId : DomainAggregateGuid
  {
    public BusinessYearId() {}
    public BusinessYearId(Guid value) : base(value) { }

    public static implicit operator BusinessYearId(Guid value) => new BusinessYearId(value);
    public static implicit operator BusinessYearId(string value) => new BusinessYearId(Guid.Parse(value));
    // private Guid _value;
    
    // public BusinessYearId(Guid value)
    // {
    //   if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
    //   _value = value;
    // }

    // public static implicit operator BusinessYearId(Guid value) => new BusinessYearId(value);
    // public static implicit operator Guid(BusinessYearId self) => self._value;
    // public override string ToString() => _value.ToString();
  }
}
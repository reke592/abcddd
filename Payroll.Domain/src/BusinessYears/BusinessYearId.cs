using System;

namespace Payroll.Domain.BusinessYears
{
  public class BusinessYearId : AggregateId<BusinessYearId>
  {
    public BusinessYearId() {}
    public BusinessYearId(Guid value)
    {
      Value = value;
    }

    public static implicit operator BusinessYearId(Guid value) => new BusinessYearId(value);
    public static implicit operator BusinessYearId(string value) => new BusinessYearId(Guid.Parse(value));
  }
}
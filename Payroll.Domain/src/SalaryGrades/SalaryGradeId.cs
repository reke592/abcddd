using System;

namespace Payroll.Domain.SalaryGrades
{
  public class SalaryGradeId : AggregateId<SalaryGradeId>
  {
    public SalaryGradeId() {}
    public SalaryGradeId(Guid value)
    {
      Value = value;
    }

    public static implicit operator SalaryGradeId(Guid value) => new SalaryGradeId(value);
    public static implicit operator SalaryGradeId(string value) => new SalaryGradeId(Guid.Parse(value));
  }
}
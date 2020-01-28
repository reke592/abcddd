using System;

namespace Payroll.Domain.SalaryGrades
{
  public class SalaryGradeId : DomainAggregateGuid
  {
    public SalaryGradeId() {}
    public SalaryGradeId(Guid value) : base(value) { }

    public static implicit operator SalaryGradeId(Guid value) => new SalaryGradeId(value);
    public static implicit operator SalaryGradeId(string value) => new SalaryGradeId(Guid.Parse(value));
    // private Guid _value;

    // public SalaryGradeId(Guid value)
    // {
    //   if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
    //   _value = value;
    // }

    // public static implicit operator SalaryGradeId(Guid value) => new SalaryGradeId(value);
    // public static implicit operator Guid(SalaryGradeId self) => self._value;
    // public override string ToString() => _value.ToString();
  }
}
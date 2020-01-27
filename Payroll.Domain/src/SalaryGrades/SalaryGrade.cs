using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Users;

namespace Payroll.Domain.SalaryGrades
{
  public class SalaryGrade : Aggregate
  {
    public UserId Owner { get; private set; }
    public BusinessYearId BusinessYear { get; private set; }
    public decimal Gross { get; private set; }

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.SalaryGradeCreated x:
          Id = x.Id;
          Owner = x.CreatedBy;
          BusinessYear = x.BusinessYear;
          Gross = x.GrossValue;
          break;
        
        case Events.V1.SalaryGradeGrossUpdated x:
          Gross = x.NewGrossValue;
          break;
      }
    }

    public static SalaryGrade Create(SalaryGradeId id, BusinessYearId currentYear, decimal grossValue, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new SalaryGrade();
      record.Apply(new Events.V1.SalaryGradeCreated
      {
        Id = id,
        BusinessYear = currentYear,
        GrossValue = grossValue,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    public void updateGross(decimal newAmount, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(this.Owner != updatedBy)
        _updateFailed("can't update salary gross. not the record owner", newAmount, updatedBy, updatedAt);
      else
        this.Apply(new Events.V1.SalaryGradeGrossUpdated {
          Id = this.Id,
          NewGrossValue = newAmount,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.SalaryGradeUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
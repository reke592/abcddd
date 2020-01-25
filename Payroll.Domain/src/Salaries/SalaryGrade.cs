using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.SalariesGrades
{
  public class SalaryGrade : Aggregate
  {
    public UserId Owner { get; private set; }
    public BusinessYearId BusinessYear { get; private set; }
    public decimal Gross { get; private set; }

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.SalaryCreated x:
          Id = x.Id;
          Owner = x.CreatedBy;
          BusinessYear = x.BusinessYear;
          break;
        
        case Events.V1.SalaryGrossUpdated x:
          Gross = x.NewGrossValue;
          break;
      }
    }

    public static SalaryGrade Create(SalaryGradeId id, BusinessYearId currentYear, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new SalaryGrade();
      record.Apply(new Events.V1.SalaryCreated
      {
        Id = id,
        BusinessYear = currentYear,
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
        this.Apply(new Events.V1.SalaryGrossUpdated {
          Id = this.Id,
          NewGrossValue = newAmount,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
        
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.SalaryUpdateFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
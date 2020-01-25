using System;
using Payroll.Domain.SalariesGrades;
using Payroll.Domain.Users;

namespace Payroll.Domain.Employees
{
  public class Employee : Aggregate
  {
    public UserId Owner { get; private set; }
    public BioData BioData { get; private set; }
    public SalaryGradeId SalaryGrade { get; private set; }
    public EmployeeStatus Status { get; private set; }

    protected override void When(object e) {
      switch(e) 
      {
        case Events.V1.EmployeeCreated x:
          Id = x.Id;
          Owner = x.CreatedBy;
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          BioData = x.BioData;
          break;
      }
    }

    public static Employee Create(EmployeeId id, UserId owner, DateTimeOffset createdAt)
    {
      var record = new Employee();
      record.Apply(new Events.V1.EmployeeCreated {
        Id = id,
        CreatedBy = owner,
        CreatedAt = createdAt
      });
      return record;
    }

    public void changeStatus(EmployeeStatus newStatus, UserId changedBy, DateTimeOffset changedAt)
    {
      if(this.Owner != changedBy)
        _updateFailed("can't change status. not the record owner", newStatus, changedBy, changedAt);
      else
        this.Apply(new Events.V1.EmployeeStatusChanged {
          Id = this.Id,
          NewStatus = newStatus,
          ChangedBy = changedBy,
          ChangedAt = changedAt
        });
    }

    public void updateBioData(BioData bioData, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(this.Owner != updatedBy)
        _updateFailed("not the record owner", bioData, updatedBy, updatedAt);
      else
        this.Apply(new Events.V1.EmployeeBioDataUpdated {
          Id = this.Id,
          BioData = bioData,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
    }

    public void updateSalaryGrade(SalaryGradeId salaryGrade, UserId updatedBy, DateTimeOffset updatedAt)
    {
      this.Apply(new Events.V1.EmployeeSalaryGradeUpdated {
        Id = this.Id,
        SalaryGrade = salaryGrade,
        UpdatedBy = updatedBy,
        UpdatedAt = updatedAt
      });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.EmployeeUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
using System;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.Salaries
{
  public class Salary : Aggregate
  {
    public EmployeeId Employee { get; private set; }
    public UserId Owner { get; private set; }
    public decimal Gross { get; private set; }

    protected override void When(object e) {
      throw new System.NotImplementedException();
    }

    public static Salary Create(SalaryId id, EmployeeId employee, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new Salary();
      record.Apply(new Events.V1.SalaryCreated
      {
        Id = id,
        Employee = employee,
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
using System;
using Payroll.Domain.Users;

namespace Payroll.Domain.Employees
{
  public class Employee : Aggregate
  {
    public UserId Owner { get; private set; }
    public BioData BioData { get; private set; }

    protected override void When(object e) {
      throw new System.NotImplementedException();
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

    public void updateBio(BioData bioData, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(this.Owner != updatedBy)
        _updateFailed("not the record owner", bioData, updatedBy, updatedAt);
      else
        this.Apply(new Events.V1.BioDataUpdated {
          Id = this.Id,
          BioData = bioData,
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
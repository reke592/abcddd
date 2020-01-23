using System;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.Deductions
{
  public class Deduction : Aggregate
  {
    public EmployeeId Employee { get; private set; }
    public UserId Owner { get; private set; }
    public decimal Balance { get; private set; }

    public bool HasBalance => this.Balance > 0;

    protected override void When(object e) {
      throw new System.NotImplementedException();
    }

    public static Deduction Create(DeductionId id, EmployeeId employee, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new Deduction();
      record.Apply(new Events.V1.DeductionCreated {
        Id = id,
        Employee = employee,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    public void setBalance(decimal newAmount, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(this.Owner != updatedBy)
        _updateFailed("can't update deduction balance. not the record owner", newAmount, updatedBy, updatedAt);
      else
        this.Apply(new Events.V1.DeductionBalanceUpdated {
          Id = this.Id,
          NewBalance = newAmount,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
    }

    public void createPayment(decimal paymentAmount, UserId createdBy, DateTimeOffset createdAt)
    {
      if(this.Owner != createdBy)
      {
        _updateFailed("can't create deduction payment. not the record owner", paymentAmount, createdBy, createdAt);
        return;
      }
        
      if(this.HasBalance == false)
      {
        _updateFailed("can't create deduction payment. No balance to pay", paymentAmount, createdBy, createdAt);
        return;
      }

      this.Apply(new Events.V1.DeductionPaymentCreated {
        Id = this.Id,
        PaidAmount = paymentAmount,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.DeductionUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
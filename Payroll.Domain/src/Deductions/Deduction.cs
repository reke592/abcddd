using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.Deductions
{
  public class Deduction : Aggregate
  {
    public UserId Owner { get; private set; }
    public EmployeeId Employee { get; private set; }
    public BusinessYearId BusinessYear { get; set; }
    public DeductionSchedule Schedule { get; private set; }
    public int Amortization { get; private set; }
    public decimal Balance { get; private set; }
    public decimal Payments { get; private set; }
    public bool Completed { get; private set; }
    public bool HasBalance => this.Balance > 0;

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.DeductionCreated x:
          Id = x.Id;
          BusinessYear = x.BusinessYear;
          Owner = x.CreatedBy;
          Employee = x.Employee;
          break;
        
        // case Events.V1.DeductionAmountSettled x:
        //   Balance = x.NewAmount;
        //   break;

        case Events.V1.DeductionScheduleSettled x:
          Amortization = x.NewAmortization;
          Balance = x.AmortizedAmount * x.NewAmortization;
          Schedule = x.NewSchedule;
          break;

        case Events.V1.DeductionPaymentCreated x:
          Balance = Balance - x.PaidAmount;
          Payments = Payments + x.PaidAmount;
          break;
        
        case Events.V1.DeductionPaymentCompleted x:
          Completed = true;
          break;
      }
    }

    public static Deduction Create(DeductionId id, EmployeeId employee, BusinessYearId businessYear, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new Deduction();
      record.Apply(new Events.V1.DeductionCreated {
        Id = id,
        Employee = employee,
        BusinessYear = businessYear,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    // public void setAmount(decimal newAmount, UserId settledBy, DateTimeOffset settledAt)
    // {
    //   if(this.Owner != settledBy)
    //     _updateFailed("can't update deduction amount. not the record owner", newAmount, settledBy, settledAt);
    //   else
    //     this.Apply(new Events.V1.DeductionAmountSettled {
    //       Id = this.Id,
    //       NewAmount = newAmount,
    //       SettledBy = settledBy,
    //       SettledAt = settledAt
    //     });
    // }

    public void setSchedule(int amortization, decimal amortizedAmount, DeductionSchedule schedule, UserId settledBy, DateTimeOffset settledAt)
    {
      if(this.Owner != settledBy)
      {
        _updateFailed("can't set deduction schedule. not the record owner", new { amortization, amortizedAmount, schedule }, settledBy, settledAt);
      }
      else if(amortization < 0)
      {
        _updateFailed("can't set amortization. invalid amortization value", amortization, settledBy, settledAt);
      }
      else
        this.Apply(new Events.V1.DeductionScheduleSettled {
          Id = this.Id,
          NewSchedule = schedule,
          NewAmortization = amortization,
          AmortizedAmount = amortizedAmount,
          SettledBy = settledBy,
          SettledAt = settledAt
        });
    }

    public void createPayment(decimal paymentAmount, BusinessYearId currentYear, UserId createdBy, DateTimeOffset createdAt)
    {
      if(this.Owner != createdBy)
        _updateFailed("can't create deduction payment. not the record owner", paymentAmount, createdBy, createdAt);
      else if(this.HasBalance == false)
        _updateFailed("can't create deduction payment. No balance to pay", paymentAmount, createdBy, createdAt);
      else
      {
        this.Apply(new Events.V1.DeductionPaymentCreated {
          Id = this.Id,
          PaidAmount = paymentAmount,
          CreatedBy = createdBy,
          CreatedAt = createdAt
        });

        if(this.HasBalance == false)
          StopDeduction(currentYear, createdBy, createdAt);
      }
    }

    public void StopDeduction(BusinessYearId currentYear, UserId haltedBy, DateTimeOffset haltedAt)
    {
      if(Completed)
        _updateFailed("can't stop deduction, deduction already completed", currentYear, haltedBy, haltedAt);
      else
        this.Apply(new Events.V1.DeductionPaymentCompleted {
          Id = this.Id,
          BusinessYear = currentYear,
          PaymentTotal = Payments,
          SettledBy = haltedBy,
          CompletedAt = haltedAt
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
using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Deductions;
using Payroll.EventSourcing;
using DeductionEvents = Payroll.Domain.Deductions.Events.V1;

namespace Payroll.Application.Deductions.Projections
{
  public class NonMandatoryDeductionProjection : IProjection
  {
    public class NonMandatoryDeductionRecord
    {
      public DeductionId DeductionId { get; internal set; }
      public BusinessYearId BusinessYear { get; internal set; }
      public DeductionSchedule Schedule { get; internal set; }
      public decimal AmortizedAmount { get; internal set; } = 0;
      public int Amortization { get; internal set; } = 0;
      public decimal Balance { get; internal set; } = 0;
      public decimal TotalPaid { get; internal set; } = 0;
      public DateTimeOffset CreatedAt { get; internal set; }
      public DateTimeOffset LastUpdated { get; internal set; }
    }

    public void Handle(object e, ICacheStore db) {
      NonMandatoryDeductionRecord doc;
      switch(e)
      {
        case DeductionEvents.NonMandatoryDeductionCreated x:
          doc = new NonMandatoryDeductionRecord();
          doc.DeductionId = x.Id;
          doc.BusinessYear = x.BusinessYear;
          doc.Schedule = x.Schedule;
          doc.CreatedAt = x.CreatedAt;
          doc.LastUpdated = x.CreatedAt;
          db.Store<NonMandatoryDeductionRecord>(x.Id, doc);
          break;

        case DeductionEvents.DeductionScheduleSettled x:
          db.UpdateIfFound<NonMandatoryDeductionRecord>(x.Id, r => {
            r.Balance = x.AmortizedAmount * x.NewAmortization;
            r.Amortization = x.NewAmortization;
            r.AmortizedAmount = x.AmortizedAmount;
            r.LastUpdated = x.SettledAt;
          });
          break;

        case DeductionEvents.DeductionPaymentCreated x:
          db.UpdateIfFound<NonMandatoryDeductionRecord>(x.Id, r => {
            r.Balance -= x.PaidAmount;
            r.TotalPaid += x.PaidAmount;
            r.LastUpdated = x.CreatedAt;
          });
          break;
      }
    }
  }
}
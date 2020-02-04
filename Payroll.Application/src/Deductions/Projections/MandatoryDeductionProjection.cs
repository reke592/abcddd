using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Deductions;
using Payroll.EventSourcing;
using DeductionEvents = Payroll.Domain.Deductions.Events.V1;

namespace Payroll.Application.Deductions.Projections
{
  public class MandatoryDeductionProjection : IProjection
  {
    public class MandatoryDeductionRecord
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
      MandatoryDeductionRecord doc;
      switch(e)
      {
        case DeductionEvents.MandatoryDeductionCreated x:
          doc = new MandatoryDeductionRecord();
          doc.DeductionId = x.Id.ToString();
          doc.BusinessYear = x.BusinessYear;
          doc.Schedule = x.Schedule;
          doc.CreatedAt = x.CreatedAt;
          doc.LastUpdated = x.CreatedAt;
          db.Store<MandatoryDeductionRecord>(x.Id, doc);
          break;

        case DeductionEvents.DeductionScheduleSettled x:
          db.UpdateIfFound<MandatoryDeductionRecord>(x.Id, r => {
            r.Balance = x.AmortizedAmount * x.NewAmortization;
            r.Amortization = x.NewAmortization;
            r.AmortizedAmount = x.AmortizedAmount;
            r.LastUpdated = x.SettledAt;
          });
          break;

        case DeductionEvents.DeductionPaymentCreated x:
          db.UpdateIfFound<MandatoryDeductionRecord>(x.Id, r => {
            r.Balance -= x.PaidAmount;
            r.TotalPaid += x.PaidAmount;
            r.LastUpdated = x.CreatedAt;
          });
          break;
      }
    }
  }
}
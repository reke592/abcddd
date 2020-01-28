using Payroll.Domain.SalaryGrades;
using Payroll.EventSourcing;
using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using SalaryGradeEvents = Payroll.Domain.SalaryGrades.Events.V1;

namespace Payroll.Application.SalaryGrades.Projections
{
  public class SalaryGradeHistoryProjection : IProjection
  {
    public class SalaryGradeRecord
    {
      public SalaryGradeId Id { get; internal set; }
      public int BusinessYear { get; internal set; }
      public decimal Gross { get; internal set; }
    }

    public void Handle(object e, ICacheStore snapshots) {
      SalaryGradeRecord doc;
      switch(e)
      {
        case SalaryGradeEvents.SalaryGradeCreated x:
          doc = new SalaryGradeRecord();
          doc.Id = x.Id;
          doc.BusinessYear = snapshots.Get<BusinessYearHistoryRecord>(x.BusinessYear).Year;
          doc.Gross = x.GrossValue;
          snapshots.Store<SalaryGradeRecord>(x.Id, doc);
          break;
        
        case SalaryGradeEvents.SalaryGradeGrossUpdated x:
          snapshots.UpdateIfFound<SalaryGradeRecord>(x.Id, r => r.Gross = x.NewGrossValue);
          break;
      }
    }
  }
}
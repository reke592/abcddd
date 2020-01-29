using System.Collections.Generic;
using Payroll.Domain.BusinessYears;
using Payroll.EventSourcing;
using BusinessYearEvents = Payroll.Domain.BusinessYears.Events.V1;

namespace Payroll.Application.BusinessYears.Projections
{
  public class BusinessYearHistoryProjection : IProjection
  {

    public class BusinessYearHistoryRecord
    {
      public BusinessYearId Id { get; internal set; }
      public ISet<ConsigneePerson> Consignees { get; internal set; } = new HashSet<ConsigneePerson>();
      public int Year { get; internal set; }
      public bool Ended { get; internal set; } = false;
    }

    public void Handle(object e, ICacheStore db) {
      BusinessYearHistoryRecord doc;
      switch(e)
      {
        case BusinessYearEvents.BusinessYearCreated x:
          doc = new BusinessYearHistoryRecord();
          doc.Id = x.Id;
          doc.Year = x.ApplicableYear;
          db.Store<BusinessYearHistoryRecord>(x.Id, doc);
          break;
        
        case BusinessYearEvents.BusinessYearConsigneeCreated x:
          db.UpdateIfFound<BusinessYearHistoryRecord>(x.Id, r => r.Consignees.Add(x.Consignee));
          break;
        
        case BusinessYearEvents.BusinessYearConsigneeUpdated x:
          db.UpdateIfFound<BusinessYearHistoryRecord>(x.Id, r => {
            r.Consignees.Remove(x.OldValue);
            r.Consignees.Add(x.NewValue);
          });
          break;
        
        case BusinessYearEvents.BusinessYearStarted x:
          doc = db.Get<BusinessYearHistoryRecord>(x.Id);
          break;
        
        case BusinessYearEvents.BusinessYearEnded x:
          db.UpdateIfFound<BusinessYearHistoryRecord>(x.Id, r => r.Ended = true);
          break;
      }
    }
  }
}
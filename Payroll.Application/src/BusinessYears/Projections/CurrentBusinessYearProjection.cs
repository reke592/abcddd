using System;
using System.Collections.Generic;
using Payroll.Domain.BusinessYears;
using Payroll.EventSourcing;
using static Payroll.Domain.BusinessYears.Events.V1;

namespace Payroll.Application.BusinessYears.Projections
{
  public class CurrentBusinessYearProjection : IProjection
  {
    public class CurrentBusinessYearRecord
    {
      public BusinessYearId Id { get; internal set; }
      public int Year { get; internal set; }
      public ISet<ConsigneePerson> Consignees { get; internal set; } = new HashSet<ConsigneePerson>();
      public bool Started { get; internal set; } = false;
      public bool Ended { get; internal set; } = false;
      public DateTimeOffset StartedAt { get; internal set; }
      public DateTimeOffset EndedAt { get; internal set; }
    }

    private static CurrentBusinessYearProjection _instance;
    private static Guid projectionId = Guid.NewGuid();

    private CurrentBusinessYearProjection() { }

    public static CurrentBusinessYearProjection Instance
      => _instance ?? (_instance = new CurrentBusinessYearProjection());

    public void Handle(object e, ICacheStore db) {
      CurrentBusinessYearRecord doc;
      switch(e)
      {
        case BusinessYearCreated x:
          db.DeleteAll<CurrentBusinessYearRecord>();
          doc = new CurrentBusinessYearRecord();
          doc.Id = x.Id;
          doc.Year = x.ApplicableYear;
          db.Store<CurrentBusinessYearRecord>(projectionId, doc);
          break;
        
        case BusinessYearConsigneeCreated x:
          db.UpdateIfFound<CurrentBusinessYearRecord>(projectionId, r => r.Consignees.Add(x.Consignee));
          break;
        
        case BusinessYearConsigneeUpdated x:
          db.UpdateIfFound<CurrentBusinessYearRecord>(projectionId, r => {
            r.Consignees.Remove(x.OldValue);
            r.Consignees.Add(x.NewValue);
          });
          break;
        
        case BusinessYearStarted x:
          db.UpdateIfFound<CurrentBusinessYearRecord>(projectionId, r => {
            r.Started = true;
            r.StartedAt = x.StartedAt;
          });
          break;

        case BusinessYearEnded x:
          db.UpdateIfFound<CurrentBusinessYearRecord>(projectionId, r => {
            r.Ended = true;
            r.EndedAt = x.EndedAt;
          });
          break;
      }
    }
  }
}
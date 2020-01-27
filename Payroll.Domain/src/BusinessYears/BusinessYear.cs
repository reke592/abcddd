using System;
using System.Collections.Generic;
using Payroll.Domain.PayrollPeriods;
using Payroll.Domain.Users;

namespace Payroll.Domain.BusinessYears
{
  public class BusinessYear : Aggregate
  {
    public int ApplicableYear { get; private set; }
    public UserId Owner { get; private set; }
    public bool Started { get; private set; }
    public bool Ended { get; private set; }
    private IList<ConsigneePerson> _consigneeList = new List<ConsigneePerson>();
    private IList<PayrollPeriod> _createdPayrolls = new List<PayrollPeriod>();

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.BusinessYearCreated x:
          Id = x.Id;
          ApplicableYear = x.ApplicableYear;
          Owner = x.CreatedBy;
          break;

        case Events.V1.BusinessYearStarted x:
          Started = true;
          break;

        case Events.V1.BusinessYearEnded x:
          Ended = true;
          break;
        
        case Events.V1.BusinessYearConsigneeCreated x:
          _consigneeList.Add(x.Consignee);
          break;
        
        case Events.V1.BusinessYearConsigneeUpdated x:
          _consigneeList.Remove(x.OldValue);
          _consigneeList.Add(x.NewValue);
          break;
      }
    }

    public static BusinessYear Create(BusinessYearId id, int appYear, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new BusinessYear();
      record.Apply(new Events.V1.BusinessYearCreated{
        Id = id,
        ApplicableYear = appYear,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    public void Start(UserId startedBy, DateTimeOffset startedAt)
    {
      if(Ended)
        _updateFailed("can't start business year. already ended", this.ApplicableYear, startedBy, startedAt);
      else if(Started)
        _updateFailed("can't start business year. already started", this.ApplicableYear, startedBy, startedAt);
      else
        this.Apply(new Events.V1.BusinessYearStarted {
          Id = this.Id,
          Year = this.ApplicableYear,
          StartedBy = startedBy,
          StartedAt = startedAt
        });
    }

    public void End(UserId endedBy, DateTimeOffset endedAt)
    {
      if(!Started)
        _updateFailed("can't end business year. never started", this.ApplicableYear, endedBy, endedAt);
      else if(Ended)
        _updateFailed("can't end business year. already ended", this.ApplicableYear, endedBy, endedAt);
      else
        this.Apply(new Events.V1.BusinessYearEnded {
          Id = this.Id,
          Year = this.ApplicableYear,
          EndedBy = endedBy,
          EndedAt = endedAt
        });
    }

    public void addConsignee(ConsigneePerson consignee, UserId addedBy, DateTimeOffset addedAt)
    {
      if(_consigneeList.Contains(consignee))
        _updateFailed("can't add consignee. already exist", consignee, addedBy, addedAt);
      else
        this.Apply(new Events.V1.BusinessYearConsigneeCreated {
          Id = this.Id,
          Consignee = consignee,
          AddedBy = addedBy,
          AddedAt = addedAt
        });
    }

    public void updateConsignee(ConsigneePerson record, ConsigneePerson replacement, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(!_consigneeList.Contains(record))
        _updateFailed("can't replace consignee. not exist", record, updatedBy, updatedAt);
      else if(record.Equals(replacement))
        return; // ignore
      else
        this.Apply(new Events.V1.BusinessYearConsigneeUpdated {
          Id = this.Id,
          OldValue = record,
          NewValue = replacement,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.BusinessYearUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
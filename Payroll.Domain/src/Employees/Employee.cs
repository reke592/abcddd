using System;
using Payroll.Domain.SalaryGrades;
using Payroll.Domain.Shared;
using Payroll.Domain.Users;

namespace Payroll.Domain.Employees
{
  public class Employee : Aggregate
  {
    public UserId Owner { get; private set; }
    public BioData BioData { get; private set; }
    public SalaryGradeId SalaryGrade { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public EmployeeLeaveRequest CurrentLeave { get; private set; }

    protected override void When(object e) {
      switch(e) 
      {
        case Events.V1.EmployeeCreated x:
          Id = x.Id;
          BioData = x.BioData;
          Owner = x.CreatedBy;
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          BioData = x.BioData;
          break;
        
        case Events.V1.EmployeeEmployed x:
          Status = x.NewStatus;
          break;

        case Events.V1.EmployeeSeparated x:
          Status = x.NewStatus;
          break;

        case Events.V1.EmployeeLeaveGranted x:
          Status = EmployeeStatus.ON_LEAVE;
          CurrentLeave = x.LeaveRequest;
          break;

        case Events.V1.EmployeeLeaveRevoked x:
          Status = EmployeeStatus.EMPLOYED;
          CurrentLeave = null;
          break;
          
        case Events.V1.EmployeeLeaveEnded x:
          Status = EmployeeStatus.EMPLOYED;
          CurrentLeave = null;
          break;
        
        case Events.V1.EmployeeSalaryGradeUpdated x:
          SalaryGrade = x.SalaryGradeId;
          break;
      }
    }

    public static Employee Create(EmployeeId id, BioData bioData, UserId owner, DateTimeOffset employedAt)
    {
      var record = new Employee();
      record.Apply(new Events.V1.EmployeeCreated {
        Id = id,
        BioData = bioData,
        CreatedBy = owner,
        CreatedAt = employedAt
      });
      return record;
    }

    public void markEmployed(UserId settledBy, DateTimeOffset settledAt)
    {
      if(this.Owner != settledBy)
        _updateFailed("can't mark employed. not the record owner", null, settledBy, settledAt);
      else
        this.Apply(new Events.V1.EmployeeEmployed {
          Id = this.Id,
          BioData = this.BioData,
          NewStatus = EmployeeStatus.EMPLOYED,
          EmployedBy = settledBy,
          EmployedAt = settledAt
        });
    }

    public void markSeparated(UserId settledBy, DateTimeOffset settledAt)
    {
      if(this.Owner != settledBy)
        _updateFailed("can't mark separated. not the record owner", null, settledBy, settledAt);
      else
        this.Apply(new Events.V1.EmployeeSeparated {
          Id = this.Id,
          BioData = this.BioData,
          NewStatus = EmployeeStatus.SEPARATED,
          SeparatedBy = settledBy,
          SeparatedAt = settledAt
        });
    }

    public void grantLeave(Date start, Date end, UserId grantedBy, DateTimeOffset grantedAt)
    {
      var request = EmployeeLeaveRequest.Create(start, end);
      if(this.Owner != grantedBy)
        _updateFailed("can't grant leave. not the record owner", request, grantedBy, grantedAt);
      else if(Status == EmployeeStatus.SEPARATED)
        _updateFailed("can't grant leave. employee is separated", request, grantedBy, grantedAt);
      else if(CurrentLeave != null)
        _updateFailed("can't grant leave. employee has active leave", request, grantedBy, grantedAt);
      else if(start.isPast() || end.isPast() || Date.Compare(start, end) < 0)
        _updateFailed("can't grant leave. invalid request", request, grantedBy, grantedAt);
      else
        this.Apply(new Events.V1.EmployeeLeaveGranted {
          Id = this.Id,
          LeaveRequest = request,
          GrantedBy = grantedBy,
          GrantedAt = grantedAt
        });
    }

    public void revokeLeave(UserId revokedBy, DateTimeOffset revokedAt)
    {
      if(this.Owner != revokedBy)
        _updateFailed("can't revoke leave. not the record owner", CurrentLeave, revokedBy, revokedAt);
      if(CurrentLeave is null)
        _updateFailed("can't revoke leave. employee has no current leave", CurrentLeave, revokedBy, revokedAt);
      else
        this.Apply(new Events.V1.EmployeeLeaveRevoked{
          Id = this.Id,
          RevokedLeaveRequest = CurrentLeave,
          RevokedBy = revokedBy,
          RevokedAt = revokedAt
        });
    }

    public void endLeave(UserId endedBy, DateTimeOffset endedAt)
    {
      if(this.Owner != endedBy)
        _updateFailed("can't end employee leave. not the record owner", CurrentLeave, endedBy, endedAt);
      if(CurrentLeave is null)
        _updateFailed("can't end employee leave. employee has no current leave", CurrentLeave, endedBy, endedAt);
      else
        this.Apply(new Events.V1.EmployeeLeaveEnded{
          Id = this.Id,
          EndedLeaveRequest = CurrentLeave,
          EndedBy = endedBy,
          EndedAt = endedAt
        });
    }

    public void updateBioData(BioData bioData, UserId updatedBy, DateTimeOffset updatedAt)
    {
      if(this.Owner != updatedBy)
        _updateFailed("not the record owner", bioData, updatedBy, updatedAt);
      else
        this.Apply(new Events.V1.EmployeeBioDataUpdated {
          Id = this.Id,
          BioData = bioData,
          UpdatedBy = updatedBy,
          UpdatedAt = updatedAt
        });
    }

    public void updateSalaryGrade(SalaryGradeId salaryGrade, UserId updatedBy, DateTimeOffset updatedAt)
    {
      this.Apply(new Events.V1.EmployeeSalaryGradeUpdated {
        Id = this.Id,
        SalaryGradeId = salaryGrade,
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
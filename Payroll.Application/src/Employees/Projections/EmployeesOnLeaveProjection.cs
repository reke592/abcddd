using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;

namespace Payroll.Application.Employees.Projections
{
  public class EmployeesOnLeaveProjection : IProjection
  {
    public class OnLeaveEmployeeRecord
    {

      public EmployeeId Id { get; internal set; }
      public BioData BioData { get; internal set; }
      public EmployeeStatus Status { get; internal set; } = EmployeeStatus.ON_LEAVE;
      public string Start { get; internal set; }
      public string Return { get; internal set; }
    }

    public void Handle(object e, ISnapshotStore snapshots) {
      OnLeaveEmployeeRecord doc;
      switch(e)
      {
        case Events.V1.EmployeeLeaveGranted x:
          doc = new OnLeaveEmployeeRecord();
          doc.Id = x.Id;
          doc.BioData = x.BioData;
          doc.Start = x.LeaveRequest.Start;
          doc.Return = x.LeaveRequest.Return;
          snapshots.Store<OnLeaveEmployeeRecord>(x.Id, doc);
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          snapshots.UpdateIfFound<OnLeaveEmployeeRecord>(x.Id, r => r.BioData = x.BioData);
          break;

        case Events.V1.EmployeeLeaveEnded x:
          snapshots.Delete<OnLeaveEmployeeRecord>(x.Id);
          break;

        case Events.V1.EmployeeLeaveRevoked x:
          snapshots.Delete<OnLeaveEmployeeRecord>(x.Id);
          break;
      }
    }
  }
}
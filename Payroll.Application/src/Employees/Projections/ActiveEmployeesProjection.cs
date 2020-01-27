using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;

namespace Payroll.Application.Employees.Projections
{
  public class ActiveEmployeesProjection : IProjection
  {
    public class ActiveEmployeeRecord
    {
      public EmployeeId Id { get; internal set; }
      public BioData BioData { get; internal set; }
      public EmployeeStatus Status { get; internal set; } = EmployeeStatus.EMPLOYED;
    }
    
    public void Handle(object e, ISnapshotStore snapshots) {
      ActiveEmployeeRecord active_doc;
      switch(e)
      {
        case Events.V1.EmployeeStatusEmployed x:
          active_doc = new ActiveEmployeeRecord();
          active_doc.Id = x.Id;
          active_doc.BioData = x.BioData;
          snapshots.Store<ActiveEmployeeRecord>(x.Id, active_doc);
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          snapshots.UpdateIfFound<ActiveEmployeeRecord>(x.Id, r => r.BioData = x.BioData);
          break;

        case Events.V1.EmployeeStatusSeparated x:
          snapshots.Delete<ActiveEmployeeRecord>(x.Id);
          break;
      }
    }
  }
}
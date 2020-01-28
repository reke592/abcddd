using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;

namespace Payroll.Application.Employees.Projections
{
  public class SeparatedEmployeesProjection : IProjection
  {
    public class SeparatedEmployeeRecord
    {
      public EmployeeId Id { get; internal set; }
      public BioData BioData { get; internal set; }
      public EmployeeStatus Status { get; internal set; } = EmployeeStatus.SEPARATED;
      public Date DateSeparated { get; internal set; }
    }

    
    public void Handle(object e, ICacheStore snapshots) {
      SeparatedEmployeeRecord doc;
      switch(e)
      {
        case Events.V1.EmployeeCreated x:
          doc = new SeparatedEmployeeRecord();
          doc.Id = x.Id;
          doc.DateSeparated = Date.TryParse(x.CreatedAt.ToString());
          snapshots.Store<SeparatedEmployeeRecord>(x.Id, doc);
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          snapshots.UpdateIfFound<SeparatedEmployeeRecord>(x.Id, r => r.BioData = x.BioData);
          break;

        case Events.V1.EmployeeStatusSeparated x:
          doc = new SeparatedEmployeeRecord();
          doc.Id = x.Id;
          doc.BioData = x.BioData;
          doc.DateSeparated = Date.TryParse(x.SettledAt.ToString());
          snapshots.Store<SeparatedEmployeeRecord>(x.Id, doc);
          break;

        case Events.V1.EmployeeStatusEmployed x:
          snapshots.Delete<SeparatedEmployeeRecord>(x.Id);
          break;
      }
    }
  }
}
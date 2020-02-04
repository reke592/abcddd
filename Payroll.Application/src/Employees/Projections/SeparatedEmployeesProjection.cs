using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;

namespace Payroll.Application.Employees.Projections
{
  public class SeparatedEmployeesProjection : IProjection
  {
    public class SeparatedEmployeeRecord
    {
      public EmployeeId EmployeeId { get; internal set; }
      public BioData BioData { get; internal set; }
      public EmployeeStatus Status { get; internal set; } = EmployeeStatus.SEPARATED;
      public string DateSeparated { get; internal set; }
    }

    
    public void Handle(object e, ICacheStore snapshots) {
      SeparatedEmployeeRecord doc;
      switch(e)
      {
        case Events.V1.EmployeeSeparated x:
          doc = new SeparatedEmployeeRecord();
          doc.EmployeeId = x.Id;
          doc.BioData = x.BioData;
          doc.DateSeparated = x.SeparatedAt.ToString("MM/dd/yyyy");
          snapshots.Store<SeparatedEmployeeRecord>(x.Id, doc);
          break;

        case Events.V1.EmployeeBioDataUpdated x:
          snapshots.UpdateIfFound<SeparatedEmployeeRecord>(x.Id, r => r.BioData = x.BioData);
          break;

        case Events.V1.EmployeeEmployed x:
          snapshots.Delete<SeparatedEmployeeRecord>(x.Id);
          break;
      }
    }
  }
}
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units.application.Employees.Projections
{
    
    public class ActiveEmployeeDocument
    {
        public Bio Bio { get; set; }
        public EmployeeStatus Status { get; set; }
    }

    public class ActiveEmployeesProjection : IProjection
    {
        public void Handle(object e, ISnapshotStore snapshots)
        {
            ActiveEmployeeDocument doc;
            switch(e)
            {
                case Events.V1.EmployeeActivated x:
                    doc = new ActiveEmployeeDocument();
                    doc.Bio = x.Bio;
                    doc.Status = x.Status;
                    snapshots.Store(x.Id, doc);
                    break;

                case Events.V1.EmployeeBioUpdated x:
                    snapshots.UpdateIfFound<ActiveEmployeeDocument>(x.Id, r => r.Bio = x.Bio);
                    break;

                case Events.V1.EmployeeDeactivated x:
                    snapshots.Delete<ActiveEmployeeDocument>(x.Id);
                    break;
            }
        }
    }

}
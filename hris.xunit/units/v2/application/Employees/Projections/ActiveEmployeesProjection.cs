using System;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units.v2.application.Employees.Projections
{
    
    public class ActiveEmployeeDocument
    {
        public Guid ActivatedBy { get; set; }
        public DateTimeOffset ActivatedAt { get; set; }
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
                case Events.V2.EmployeeActivated x:
                    doc = new ActiveEmployeeDocument();
                    doc.Bio = x.Bio;
                    doc.Status = x.Status;
                    doc.ActivatedBy = x.ActivatedBy;
                    doc.ActivatedAt = x.ChangedAt;
                    snapshots.Store(x.Id, doc);
                    break;

                case Events.V2.EmployeeBioUpdated x:
                    snapshots.UpdateIfFound<ActiveEmployeeDocument>(x.Id, r => r.Bio = x.Bio);
                    break;

                case Events.V2.EmployeeDeactivated x:
                    snapshots.Delete<ActiveEmployeeDocument>(x.Id);
                    break;
            }
        }
    }

}
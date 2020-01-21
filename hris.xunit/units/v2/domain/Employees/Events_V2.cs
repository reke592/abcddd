using System;

namespace hris.xunit.units.domain.Employees
{
    public static partial class Events
    {
        // assume that an aggregate event stream already exist
        // and we need to add security layer for the next version
        public static class V2
        {
            public class EmployeeCreated : Events.V1.EmployeeCreated
            {
                public Guid CreatedBy { get; set; }
            }

            public class EmployeeBioUpdated : Events.V1.EmployeeBioUpdated
            {
                public Guid UpdatedBy { get; set; }
            }

            public class EmployeeActivated : Events.V1.EmployeeActivated
            {
                public Guid ActivatedBy { get; set; }
            }

            public class EmployeeDeactivated : Events.V1.EmployeeDeactivated
            {
                public Guid DeactivatedBy { get; set; }
            }

            public class EmployeeLeaveGranted : Events.V1.EmployeeLeaveGranted
            {
                public Guid GrantedBy { get; set; }
            }

            public class UpdateAttemptFailed
            {
                public string Reason { get; set; }
                public Guid AggregateId { get; set; }
                public Guid AttemptedBy { get; set; }
                public object AttemptedValue { get; set; }
                public DateTimeOffset AttemptedAt { get; set; }
            }

            // superuser actions
            // MUST validate if the one who committed this event is an admin
            // will use this to make sure the aggregate is still compatible
            public class OwnerChanged
            {
                public Guid NewOwner { get; set; }
                public Guid ChangedBy { get; set; }
                public DateTimeOffset ChangedAt { get; set; }
            }
        }
    }
}
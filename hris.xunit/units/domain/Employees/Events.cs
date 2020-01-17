using System;

namespace hris.xunit.units.domain.Employees {
    public static class Events
    {
        public static class V1
        {
            public class EmployeeCreated
            {
                public Guid Id { get; set; }
                public DateTimeOffset CreatedAt { get; set; }
            }

            public class EmployeeBioUpdated
            {
                public Guid Id { get; set; }
                public Bio Bio { get; set; }
                public DateTimeOffset UpdatedAt { get; set; }
            }

            public class EmployeeActivated
            {
                public Guid Id { get; set; }
                public Bio Bio { get; set; }
                public EmployeeStatus Status { get; set; }
                public DateTimeOffset ChangedAt { get; set; }
            }

            public class EmployeeDeactivated
            {
                public Guid Id { get; set; }
                public Bio Bio { get; set; }
                public EmployeeStatus Status { get; set; }
                public DateTimeOffset ChangedAt { get; set; }
            }

            public class EmployeeLeaveGranted
            {
                public Guid Id { get; set; }
                public Bio Bio { get; set; }
                public EmployeeStatus Status { get; set; }
                public DateTimeOffset GrantedAt { get; set; }
            }
        }
    }
}
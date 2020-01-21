using System;

namespace hris.xunit.units.domain.Employees {
    public class Employee : Aggregate {
        public Bio Bio { get; private set; }
        public EmployeeStatus Status { get; private set; }
        
        // v2
        public Guid CreatedBy { get; private set;}

        protected override void When(object e)
        {
            switch(e)
            {
                case Events.V1.EmployeeCreated x:
                    Id = new EmployeeId(x.Id);
                    break;

                case Events.V1.EmployeeBioUpdated x:
                    Bio = new Bio(x.Bio.FirstName, x.Bio.MiddleName, x.Bio.LastName, x.Bio.DateOfBirth);
                    break;
                
                case Events.V1.EmployeeActivated x:
                    Status = x.Status;
                    break;
            }
        }

        internal static Employee Create(EmployeeId id, DateTimeOffset createdAt) {
            var ee = new Employee();
            ee.Apply(new Events.V1.EmployeeCreated {
                Id = id,
                CreatedAt = createdAt
            });
            return ee;
        }

        internal static Employee CreateV2(EmployeeId id, Guid createdBy, DateTimeOffset createdAt)
        {
            var ee = new Employee();
            ee.Apply(new Events.V1.EmployeeCreated {
                Id = id,
                CreatedAt = createdAt
            });
            return ee;
        }

        internal void updateBio(Bio bio, DateTimeOffset updatedAt) {
            Apply(new Events.V1.EmployeeBioUpdated {
                Id = Id,
                Bio = bio,
                UpdatedAt = updatedAt
            });
        }

        internal void setActive(DateTimeOffset changedAt) {
            Apply(new Events.V1.EmployeeActivated {
                Id = Id,
                Bio = Bio,
                Status = EmployeeStatus.ACTIVE,
                ChangedAt = changedAt
            });
        }

        internal void setInactive(DateTimeOffset changedAt) {
            Apply(new Events.V1.EmployeeDeactivated {
                Id = Id,
                Bio = Bio,
                Status = EmployeeStatus.INACTIVE,
                ChangedAt = changedAt
            });
        }

        internal void leaveGranted(DateTimeOffset grantedAt) {
            Apply(new Events.V1.EmployeeLeaveGranted {
                Id = Id,
                Bio = Bio,
                Status = EmployeeStatus.ON_LEAVE,
                GrantedAt = grantedAt
            });
        }
    }
}
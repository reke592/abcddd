using System;
using hris.xunit.units.domain;
using hris.xunit.units.domain.Employees;

namespace hris.xunit.units.v2.domain.Employees {
    public class Employee : Aggregate {
        public Bio Bio { get; private set; }
        public EmployeeStatus Status { get; private set; }
        
        // v2
        public Guid CreatedBy { get; private set;}

        protected override void When(object e)
        {
            switch(e)
            {
                case Events.V2.EmployeeCreated x:
                    Id = new EmployeeId(x.Id);
                    CreatedBy = x.CreatedBy;
                    break;
                
                case Events.V2.EmployeeBioUpdated x:
                    Bio = x.Bio;
                    break;

                // --------------
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

        internal static Employee Create(EmployeeId id, Guid createdBy, DateTimeOffset createdAt)
        {
            var ee = new Employee();
            ee.Apply(new Events.V2.EmployeeCreated {
                Id = id,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });
            return ee;
        }

        internal void updateBio(Bio bio, Guid updatedBy, DateTimeOffset updatedAt) {
            if(CreatedBy != updatedBy)
                _fail_update("not the record owner", bio, updatedBy, updatedAt);
            else
                Apply(new Events.V2.EmployeeBioUpdated{
                    Bio = bio,
                    UpdatedBy = updatedBy,
                    UpdatedAt = updatedAt
                });
        }

        internal void setActive(Guid activatedBy, DateTimeOffset activatedAt) {
            if(CreatedBy != activatedBy)
                _fail_update("not the record owner", EmployeeStatus.ACTIVE, activatedBy, activatedAt);
            else
                Apply(new Events.V2.EmployeeActivated {
                    Id = Id,
                    Bio = Bio,
                    ActivatedBy = activatedBy,
                    Status = EmployeeStatus.ACTIVE,
                    ChangedAt = activatedAt
                });
        }

        internal void setInactive(Guid deactivatedBy, DateTimeOffset deactivatedAt) {
            if(CreatedBy != deactivatedBy)
                _fail_update("not the record owner", EmployeeStatus.INACTIVE, deactivatedBy, deactivatedAt);
            Apply(new Events.V2.EmployeeDeactivated {
                Id = Id,
                Bio = Bio,
                DeactivatedBy = deactivatedBy,
                Status = EmployeeStatus.INACTIVE,
                ChangedAt = deactivatedAt
            });
        }

        internal void leaveGranted(Guid grantedBy, DateTimeOffset grantedAt) {
            if(CreatedBy != grantedBy)
                _fail_update("not the record owner", EmployeeStatus.ON_LEAVE, grantedBy, grantedAt);
            Apply(new Events.V2.EmployeeLeaveGranted {
                Id = Id,
                Bio = Bio,
                GrantedBy = grantedBy,
                Status = EmployeeStatus.ON_LEAVE,
                GrantedAt = grantedAt
            });
        }

        private void _fail_update(string reason, object value, Guid attemptedBy, DateTimeOffset attemptedAt)
        {
                Apply(new Events.V2.UpdateAttemptFailed {
                    Reason = reason,
                    AttemptedBy = attemptedBy,
                    AttemptedValue = value,
                    AttemptedAt = attemptedAt
                });
        }
    }
}
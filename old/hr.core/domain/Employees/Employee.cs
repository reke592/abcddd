using hr.core.domain.Employees.rules;
using hr.core.helper;

namespace hr.core.domain.Employees {
    // Aggregate Root
    public class Employee : Entity {
        private long _bio_id;
        private long _department_id;
        private long _salary_grade_id;
        private long _work_schedule_id;

        public virtual Bio Bio { get; protected set; }
        public virtual EmployeeStatus Status { get; protected set; }
        public virtual Department Department { get; protected set; }
        public virtual WorkSchedule WorkSchedule { get; protected set; }

        public bool IsActive {
            get { 
                return new ActiveStatusRule().isSatisfiedBy(this);
            }
        }

        public bool CanBeCreated {
            get {
                return true;
                //return new EmployeeCanBeCreatedRule().isSatisfiedBy(this);
            }
        }

        public bool BelongsToAnyDepartment {
            get {
                return _department_id > 0;
            }
        }

        public static Employee Create(Bio bio) {
            var record = new Employee {
                Bio = bio
            };

            if(record.CanBeCreated)
                Broker.Emit(new events.EmployeeRecordCreated(record));
            else
                Broker.Emit(new events.errors.EmployeeNotCreated(record));
                
            return record;
        }
    }
}
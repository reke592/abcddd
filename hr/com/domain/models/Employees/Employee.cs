using hr.com.domain.enums;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Employees {
    // Root Aggregate
    public class Employee : Entity {
        private Salary _salary;  // reference
        public virtual Person Person { get; protected set; }    // component
        public virtual Date DateHired { get; protected set; }
        public virtual EmployeeStatus Status { get; protected set; }
        
        private void onCommandAssociateSalaryToEmployee(object sender, Command cmd) {
            if(cmd is CommandAssociateSalaryToEmployee) {
                var args = cmd as CommandAssociateSalaryToEmployee;
                if(args.Employee.Equals(this)) {
                    var previous = this._salary;
                    this._salary = args.Salary;
                    
                    EventBroker.getInstance().Emit(new EventEmployeeSalaryUpdated(this, previous));
                }
            }
        }

        private void onCommandChangeEmployeeStatus(object sender, Command cmd) {
            if(cmd is CommandChangeEmployeeStatus) {
                var args = cmd as CommandChangeEmployeeStatus;
                if(args.Employee.Equals(this)) {
                    var old_value = this.Status;
                    this.Status = args.Status;
                    EventBroker.getInstance().Emit(new EventEmployeeStatusChanged(this, old_value));
                }
            }
        }

        public Employee() {
            var broker = EventBroker.getInstance();
            broker.addCommandListener(onCommandAssociateSalaryToEmployee);
            broker.addCommandListener(onCommandChangeEmployeeStatus);
        }

        ~Employee() {
            var broker = EventBroker.getInstance();
            broker.removeCommandListener(onCommandAssociateSalaryToEmployee);
            broker.removeCommandListener(onCommandChangeEmployeeStatus);
        }

        public static Employee Create(Person person, Date dt_hired, EmployeeStatus status = EmployeeStatus.NEW_HIRED) {
            var record = new Employee {
                Person = person,
                DateHired = dt_hired,
                Status = status
            };

            EventBroker.getInstance().Emit(new EventEmployeeCreated(record));

            return record;
        }

        public virtual Salary GetSalary() {
            return this._salary;
        }
    }
}
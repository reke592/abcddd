using System;
using hr.com.domain.enums;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Employees {
    public class Employee : Entity {
        private Salary _salary;  // reference
        public virtual Person Person { get; protected set; }    // component
        public virtual Date DateHired { get; protected set; }
        public virtual EmployeeStatus Status { get; protected set; }
        
        private void onCommandAssociateSalaryToEmployee(object sender, Command cmd) {
            if(cmd is CommandAssociateSalaryToEmployee) {
                var args = cmd as CommandAssociateSalaryToEmployee;
                if(this.Equals(args.Employee)) {
                    if(this._salary != null)
                        throw new Exception("Employee already associated to salary.");
                    else
                        this._salary = args.Salary;
                }
            }
        }

        public Employee() {
            EventBroker.getInstance().addCommandListener(onCommandAssociateSalaryToEmployee);
        }

        public static Employee Create(Person person, Date dt_hired, EmployeeStatus status = EmployeeStatus.NEW_HIRED) {
            return new Employee {
                Person = person,
                DateHired = dt_hired,
                Status = status
            };
        }
    }
}
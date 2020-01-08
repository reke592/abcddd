using hr.core.application.Employees;
using hr.core.helper;

namespace hr.core.domain.Employees.events {
    public class EmployeeRecordCreated : IntegrationEvent {
        public Employee Employee { get; private set; }

        public EmployeeRecordCreated(Employee employee)
        : base(employee, typeof(EmployeeDTO), Integration.CREATED) {
            Employee = employee;
        }
    }
}
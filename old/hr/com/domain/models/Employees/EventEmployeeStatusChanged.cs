using hr.com.helper.domain;
using hr.com.domain.enums;

namespace hr.com.domain.models.Employees {
    public class EventEmployeeStatusChanged : Event {
        public Employee Employee;
        public EmployeeStatus Previous;

        public EventEmployeeStatusChanged(Employee employee, EmployeeStatus previous) {
            this.Employee = employee;
            this.Previous = previous;
        }
    }
}
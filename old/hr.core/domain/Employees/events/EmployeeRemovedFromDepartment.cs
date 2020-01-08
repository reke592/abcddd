using hr.core.helper;

namespace hr.core.domain.Employees.events {
    public class EmployeeRemovedFromDepartment : Event {
        public Employee Employee { get; private set; }
        public Department Department { get; private set; }

        public EmployeeRemovedFromDepartment(Department department, Employee employee) {
            Department = department;
            Employee = employee;
        }
    }
}
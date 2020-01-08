using hr.core.helper;

namespace hr.core.domain.Employees.events {
    public class EmployeeAddedToDepartment : Event {
        public Department Department { get; private set; }
        public Employee Employee { get; private set; }

        public EmployeeAddedToDepartment(Department department, Employee employee) {
            Department = department;
            Employee = employee;
        }
    }
}
using hr.core.helper;

namespace hr.core.domain.Employees.commands {
    public class AddEmployeeToDepartment : Command {
        public Employee Employee { get; private set; }
        public Department Department { get; private set; }

        public AddEmployeeToDepartment(Employee employee, Department department) {
            Employee = employee;
            Department = department;
        }
    }
}
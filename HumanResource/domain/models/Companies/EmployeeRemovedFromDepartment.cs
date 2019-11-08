using hr.domain.models.Employees;
using hr.helper.domain;

namespace hr.domain.models.Companies {
    public class EmployeeRemovedFromDepartment : Command {
        public Employee Employee { get; private set; }
        public Department Department { get; private set; }

        public EmployeeRemovedFromDepartment(Employee employee, Department department) {
            Employee = employee;
            Department = department;
        }
    }
}
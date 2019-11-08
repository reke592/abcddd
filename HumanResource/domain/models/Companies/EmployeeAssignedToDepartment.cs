using hr.domain.models.Employees;
using hr.helper.domain;

namespace hr.domain.models.Companies {
    public class EmployeeAssignedToDepartment : Command {
        public Employee Employee { get; private set; }
        public Department Department { get; private set; }
        
        public EmployeeAssignedToDepartment(Employee employee, Department department) {
            Employee = employee;
            Department = department;
        }
    }
}
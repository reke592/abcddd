using hr.com.helper.domain;
using hr.com.domain.enums;

namespace hr.com.domain.models.Employees {
    public class CommandChangeEmployeeStatus : Command {
        public Employee Employee { get; protected set; }
        public EmployeeStatus Status { get; protected set; }
        
        public CommandChangeEmployeeStatus(Employee employee, EmployeeStatus status) {
            this.Employee = employee;
            this.Status = status;
        }
    }
}
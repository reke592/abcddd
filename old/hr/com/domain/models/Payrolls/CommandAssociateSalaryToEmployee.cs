using hr.com.domain.models.Employees;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandAssociateSalaryToEmployee : Command {
        public Employee Employee { get; protected set; }
        public Salary Salary { get; protected set; }

        public CommandAssociateSalaryToEmployee(Salary salary, Employee employee) {
            this.Employee = employee;
            this.Salary = salary;
        }
    }
}
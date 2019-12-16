using hr.com.domain.models.Employees;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventEmployeeSalaryUpdated : Event {
        public Employee Employee { get; protected set; }
        public Salary Previous { get; protected set; }

        public EventEmployeeSalaryUpdated(Employee employee, Salary previous) {
            this.Employee = employee;
            this.Previous = previous;
        }
    }
}
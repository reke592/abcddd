using hr.core.helper;

namespace hr.core.domain.Employees.events {
    public class EmployeeBioUpdated : Event {
        public Employee Employee { get; private set; }
        public Bio Bio { get; private set; }

        public EmployeeBioUpdated(Employee employee, Bio bio) {
            Employee = employee;
            Bio = bio;
        }
    }
}
using hr.core.helper;

namespace hr.core.domain.Employees.commands {
    public class SetPresentAddress : Command {
        public Employee Employee { get; private set; }
        public Address Address { get; private set; }

        public SetPresentAddress(Employee employee, Address address) {
            Employee = employee;
            Address = address;
        }
    }
}
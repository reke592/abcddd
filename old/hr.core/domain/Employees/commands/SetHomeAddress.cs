using hr.core.helper;

namespace hr.core.domain.Employees.commands {
    public class SetHomeAddress : Command {
        public Employee Employee { get; private set; }
        public Address Address { get; private set; }

        public SetHomeAddress(Employee employee, Address address) {
            Employee = employee;
            Address = address;
        }
    }
}
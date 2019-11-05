using hr.application.Employees;
using hr.domain.shared;

namespace hr.domain.models.Employees {
    public class EmployeeServices {
        private readonly IEmployeeRepository _employees;
        public EmployeeServices(IEmployeeRepository employees) {
            this._employees = employees;
        }

        public void CreateEmployee(PersonDTO person) {
            var pDetails = Person.Create(person.Firstname, person.Middlename, person.Surname, person.Ext, person.Sex, person.Birthdate);
            var employee = Employee.Create(pDetails);
            // emit event
            // application layer listen to event, and make call for infrastracture services
            _employees.save(employee);
        }

        public void AddEmployeeAddress(Employee employee, AddressDTO addr) {
            var address = Address.Create(addr.LotBlock, addr.Street, addr.Municipality, addr.Province, addr.Country);
            employee.addAddress(address);
            // emit event
            // application layer listen to event, and make call for infrastracture services
            _employees.save(employee);
        }
    }
}
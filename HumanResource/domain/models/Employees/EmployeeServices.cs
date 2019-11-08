using System;
using hr.application.Employees;
using hr.domain.shared;

namespace hr.domain.models.Employees {
    public class EmployeeServices {
        private readonly IEmployeeRepository _employees;
        public EmployeeServices(IEmployeeRepository employees) {
            this._employees = employees;
        }

        public Employee CreateEmployee(PersonDTO input) {
            var pDetails = Person.Create(
                input.Firstname
                , input.Middlename
                , input.Surname
                , input.Ext
                , (EnumSex) Enum.Parse(typeof(EnumSex), input.Sex)
                , Date.TryParse(input.Birthdate));

            var employee = Employee.Create(pDetails);
            // emit event
            // application layer listen to event, and make call for infrastracture services
            return employee;
        }

        public void AddEmployeeAddress(Employee employee, AddressDTO addr) {
            var address = Address.Create(
                addr.LotBlock
                , addr.Street
                , addr.Municipality
                , addr.Province
                , addr.Country);
            
            employee.addAddress(address);
            // emit event
            // application layer listen to event, and make call for infrastracture services
        }
    }
}
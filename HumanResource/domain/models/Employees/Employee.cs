using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.domain.models.Companies;
using hr.domain.shared;
using hr.helper.domain;
using hr.helper.errors;

namespace hr.domain.models.Employees {
    public class Employee : Entity {
        private Person personDetails;
        private IList<Address> employeeAddresses = new List<Address>();
        private DateTime dateHired;
        private Department department;

        private void onEmployeeAssignedToDepartment(object sender, Command cmd) {
            var args = cmd as EmployeeAssignedToDepartment;
            if(this.Equals(args?.Employee)) {
                this.department = args.Department;
            }
        }

        private void onEmployeeRemovedFromDepartment(object sender, Command cmd) {
            var args = cmd as EmployeeRemovedFromDepartment;
            if(this.Equals(args?.Employee)) {
                this.department = null;
            }
        }

        public Employee() {
            EventBroker.getInstance.addCommandListener(onEmployeeAssignedToDepartment);
            EventBroker.getInstance.addCommandListener(onEmployeeRemovedFromDepartment);
        }

        public static Employee Create(Person pdetails) {
            // domain validate here ...
            var record = new Employee {
                personDetails = pdetails,
                dateHired = DateTime.Now
            };
            // event sourcing here...
            return record;
        }

        public virtual void addAddress(Address address) {
            // validate here ...
            this.employeeAddresses.Add(address);
            // event sourcing here...
        }

        public virtual ReadOnlyCollection<Address> getAddresses() {
            return new ReadOnlyCollection<Address>(this.employeeAddresses);
        }

        public virtual Person getPersonDetails() {
            return this.personDetails;
        }

        public virtual string getDateHired() {
            return this.dateHired.ToString("MM/dd/yyyy");
        }

        public virtual Department getDepartment() {
            return this.department;
        }
    }
}
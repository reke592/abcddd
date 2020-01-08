using System;
using hr.core.helper;

namespace hr.core.domain.Employees.events.errors {
    public class EmployeeNotCreated : ErrorEvent {
        public Employee Employee { get; private set; }
        // public Bio Data { get; private set; }
        public string Message { get; private set; }

        public EmployeeNotCreated(Employee employee, string message = "Can't create employee record.") {
            Employee = employee;
            Message = message;
        }
    }
}
using hr.com.domain.enums;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Employees {
    /// <summary>
    /// Implementation of domain.Employees.IEmployeeService
    /// </summary>
    public class EmployeeDomainService : IEmployeeDomainService
    {
        public Employee ChangeStatus(Employee employee, EmployeeStatus status)
        {
            EventBroker.getInstance().Command(new CommandChangeEmployeeStatus(employee, status));
            return employee;
        }

        public Employee Register(Person data, Date dt_hired, EmployeeStatus status = EmployeeStatus.NEW_HIRED)
        {
            var employee = Employee.Create(data, dt_hired ?? Date.Now, status);
            return employee;
        }
    }
}
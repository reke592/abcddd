using System.Collections.Generic;
using hr.com.domain.enums;
using hr.com.domain.shared;

namespace hr.com.domain.models.Employees {
    public interface IEmployeeDomainService {
        Employee Register(Person data, Date dt_hired, EmployeeStatus status);
        Employee ChangeStatus(Employee employee, EmployeeStatus status);
    }
}
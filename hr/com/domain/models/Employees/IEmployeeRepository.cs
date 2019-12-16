using System.Collections.Generic;
using hr.com.domain.enums;
using hr.com.helper.database;

namespace hr.com.domain.models.Employees {
    public interface IEmployeeRepository : IRepository<Employee> {
        IList<Employee> FindByStatus(EmployeeStatus status);
        IList<Employee> FetchAllActive();
        IList<Employee> FetchAllInActive();
    }
}
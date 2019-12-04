using System.Collections.Generic;
using hr.com.helper.database;

namespace hr.com.domain.models.Employees {
    public interface IEmployeeRepository : IRepository<Employee> {
        // fetch all active
    }
}
using System.Collections.Generic;
using hr.com.domain.models.Employees;
using hr.com.helper.database;

namespace hr.com.domain.models.Payrolls {
    public interface ISalaryRepository : IRepository<Salary> {
        IList<Deduction> FetchEmployeeActiveDeduction(Employee employee);
    }
}
using System.Collections.Generic;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.models.Payrolls.specs;

namespace hr.com.infrastracture.database.nhibernate.repositories {
    public class SalaryRepository : NHRepositoryBase<Salary>, ISalaryRepository
    {
        public IList<Deduction> FetchEmployeeActiveDeduction(Employee employee)
        {
            return FindAll(new SpecificationActiveEmployeeDeduction(employee));
        }
    }
}
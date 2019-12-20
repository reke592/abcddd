using System.Collections.Generic;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Employees.specs;
using NHibernate;
using NHibernate.Criterion;

namespace hr.com.infrastracture.database.nhibernate.repositories {
    public class EmployeeRepository : NHRepositoryBase<Employee>, IEmployeeRepository
    {
        public IList<Employee> FetchAllActive()
        {
            return FindAll(new SpecificationActiveEmployeeStatus());
        }

        public IList<Employee> FetchAllInActive()
        {
            var s = NHibernateHelper.GetCurrentSession();;
            return FindAll(new SpecificationInactiveEmployeeStatus());
        }

        public IList<Employee> FindByStatus(EmployeeStatus status)
        {
            return FindAll(new SpecificationEmployeeByStatus(status));
        }
    }
}
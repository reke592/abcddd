using System;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.database;

namespace hr.xunit.units
{
    public class EmployeeDomService : IEmployeeDomainService
    {
        private static readonly IRepository<Employee> employees = new DummyRepository<Employee>();

        public Employee RegisterNew(Person data)
        {
            return Employee.Create(data, Date.TryParse(DateTime.Now.ToShortDateString()));
        }
    }
}

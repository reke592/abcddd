using System;
using System.Linq.Expressions;
using hr.com.domain.enums;
using hr.com.helper.database;

namespace hr.com.domain.models.Employees.specs {
    public class SpecificationInactiveEmployeeStatus : Specification<Employee>
    {
        public override Expression<Func<Employee, bool>> toExpression()
        {
            return employee => employee.Status == EmployeeStatus.RESIGNED
                || employee.Status == EmployeeStatus.RETIRED;
        }
    }
}
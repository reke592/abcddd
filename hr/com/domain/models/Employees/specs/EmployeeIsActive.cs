using System;
using System.Linq.Expressions;
using hr.com.domain.enums;
using hr.com.helper.database;

namespace hr.com.domain.models.Employees.specs {
    public class EmployeeIsActive : Specification<Employee>
    {
        public override Expression<Func<Employee, bool>> toExpression()
        {
            return employee => employee.Status == EmployeeStatus.NEW_HIRED
                || employee.Status == EmployeeStatus.REGULAR
                || employee.Status == EmployeeStatus.PERMANENT
                || employee.Status == EmployeeStatus.ON_LEAVE;
        }
    }
}
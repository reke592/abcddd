using System;
using System.Linq.Expressions;
using hr.com.helper.database;
using hr.com.domain.enums;

namespace hr.com.domain.models.Employees.specs {
    public class SpecificationEmployeeByStatus : Specification<Employee>
    {
        private EmployeeStatus _status;

        public SpecificationEmployeeByStatus(EmployeeStatus status) {
            this._status = status;
        }

        public override Expression<Func<Employee, bool>> toExpression()
        {
            return employee => employee.Status == this._status;
        }
    }
}
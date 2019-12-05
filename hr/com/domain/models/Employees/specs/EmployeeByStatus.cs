using System;
using System.Linq.Expressions;
using hr.com.helper.database;
using hr.com.domain.enums;

namespace hr.com.domain.models.Employees.specs {
    public class EmployeeByStatus : Specification<Employee>
    {
        private EmployeeStatus _status;

        public EmployeeByStatus(EmployeeStatus status) {
            this._status = status;
        }

        public override Expression<Func<Employee, bool>> toExpression()
        {
            return employee => employee.Status == this._status;
        }
    }
}
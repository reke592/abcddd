using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Employees.rules {
    public class DepartmentCanAddEmployeeRule : Specification<Department>
    {
        public override Expression<Func<Department, bool>> toExpression()
        {
            return candidate => (candidate.Capacity + 1) <= candidate.Limit;
        }
    }
}
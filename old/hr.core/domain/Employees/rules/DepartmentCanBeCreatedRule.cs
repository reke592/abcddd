using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Employees.rules {
    public class DepartmentCanBeCreatedRule : Specification<Department>
    {
        public override Expression<Func<Department, bool>> toExpression()
        {
            return candidate => !string.IsNullOrEmpty(candidate.Name) && candidate.Capacity > 0;
        }
    }
}
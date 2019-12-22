using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Employees {
    public class ActiveStatusRule : Specification<Employee>
    {
        public override Expression<Func<Employee, bool>> toExpression()
        {
            return candidate => 
                candidate.Status != EmployeeStatus.RESIGNED
                || candidate.Status != EmployeeStatus.RETIRED;
        }
    }
}
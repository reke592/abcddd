using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Employees.rules {
    public class EmployeeCanBeAddedRule : Specification<Employee>
    {
        private static ValidBioRule _valid_bio = new ValidBioRule();
        public override Expression<Func<Employee, bool>> toExpression()
        {
            return candidate => _valid_bio.isSatisfiedBy(candidate.Bio);
        }
    }
}
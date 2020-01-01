using System;
using System.Linq.Expressions;
using hr.core.infrastracture;
using hr.core.domain.shared;

namespace hr.core.domain.Employees.rules {
    public class ValidBioRule : Specification<Bio>
    {
        public override Expression<Func<Bio, bool>> toExpression()
        {
            return candidate => 
                (Date.Now.Year - candidate.Birthdate.Year) >= 18
                && Enum.IsDefined(typeof(Gender), candidate.Gender)
                && !string.IsNullOrEmpty(candidate.FirstName)
                && !string.IsNullOrEmpty(candidate.LastName);
        }
    }
}
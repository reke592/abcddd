using System;
using System.Linq.Expressions;
using hr.core.infrastracture;
using hr.core.domain.commons;

namespace hr.core.domain.Employees.rules {
    public class ValidBioRule : Specification<Bio>
    {
        private static ValidAddressRule _valid_address = new ValidAddressRule();
        public override Expression<Func<Bio, bool>> toExpression()
        {
            return candidate => 
                (Date.Now.Year - (candidate.Birthdate ?? Date.Now).Year) >= 18
                && Enum.IsDefined(typeof(Gender), candidate.Gender)
                && !string.IsNullOrEmpty(candidate.FirstName)
                && !string.IsNullOrEmpty(candidate.LastName)
                && _valid_address.isSatisfiedBy(candidate.PresentAddress);
        }
    }
}
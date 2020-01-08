using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Employees.rules {
    public class ValidAddressRule : Specification<Address>
    {
        public override Expression<Func<Address, bool>> toExpression()
        {
            return (candidate) => 
                !(
                    string.IsNullOrEmpty(candidate.Province)
                    && string.IsNullOrEmpty(candidate.Municipality)
                );
        }
    }
}
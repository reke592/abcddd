using System;
using System.Linq.Expressions;
using hr.core.infrastracture;
using hr.core.domain.shared;

namespace hr.core.domain.Payrolls {
    public class MinimumWageRule : Specification<MonetaryValue>
    {
        // must be automated using a lookup table
        public static decimal MIN_WAGE = 349.0m;
        
        public override Expression<Func<MonetaryValue, bool>> toExpression()
        {
            return candidate => candidate.PreciseValue >= MIN_WAGE;
        }
    }
}
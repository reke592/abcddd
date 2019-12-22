using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Payrolls {
    public class CanAddDeductionRule : Specification<Deduction>
    {
        public override Expression<Func<Deduction, bool>> toExpression()
        {
            return candidate => candidate.Account != null
                && candidate.Total.PreciseValue > 0
                && candidate.Amortization > 0;
        }
    }
}
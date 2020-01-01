using System;
using System.Linq.Expressions;
using hr.core.domain.Deductions;
using hr.core.infrastracture;

namespace hr.core.domain.Payrolls.rules {
    public class CanGrantDeductionRule : Specification<Deduction>
    {
        public override Expression<Func<Deduction, bool>> toExpression()
        {
            return candidate => true;
        }
    }
}
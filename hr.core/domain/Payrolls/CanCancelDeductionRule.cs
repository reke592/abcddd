using System;
using System.Linq.Expressions;
using hr.core.infrastracture;

namespace hr.core.domain.Payrolls {
    public class CanCancelDeductionRule : Specification<Deduction>
    {
        public override Expression<Func<Deduction, bool>> toExpression()
        {
            return candidate => !candidate.CanBeGranted;
        }
    }
}
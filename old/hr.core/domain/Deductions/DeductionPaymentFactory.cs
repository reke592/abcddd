using hr.core.domain.Payrolls;
using hr.core.domain.commons;

namespace hr.core.domain.Deductions {
    public sealed class DeductionPaymentFactory {
        public IDeductionPayment Create(Deduction deduction, MonetaryValue amount) {
            return new CustomDeductionPayment(deduction, amount);
        }

        public IDeductionPayment Create(SalaryPayment salary, Deduction deduction) {
            return new SalaryDeductionPayment(salary, deduction);
        }
    }
}
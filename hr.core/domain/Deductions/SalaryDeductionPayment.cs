using hr.core.domain.Payrolls;
using hr.core.domain.shared;

namespace hr.core.domain.Deductions {
    public class SalaryDeductionPayment : Entity, IDeductionPayment {
        protected long _deduction_id;
        private long _salary_payment_id;
        public MonetaryValue Amount { get; protected set; }

        public SalaryDeductionPayment(SalaryPayment salary_payment, Deduction deduction) {
            _salary_payment_id = salary_payment.Id;
            _deduction_id = deduction.Id;
        }
    }
}
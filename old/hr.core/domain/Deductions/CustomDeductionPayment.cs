using hr.core.domain.commons;

namespace hr.core.domain.Deductions {
    public class CustomDeductionPayment : Entity, IDeductionPayment {
        protected long _deduction_id;
        public MonetaryValue Amount { get; protected set; }     // component

        public CustomDeductionPayment(Deduction deduction, MonetaryValue amount) {
            _deduction_id = deduction.Id;
            Amount = amount;
        }
    }
}
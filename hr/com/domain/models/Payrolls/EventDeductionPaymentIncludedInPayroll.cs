using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventDeductionPaymentIncludedInPayroll : Event {
        public PayrollReport PayrollReport { get; protected set; }
        public DeductionPayment DeductionPayment { get; protected set; }

        public EventDeductionPaymentIncludedInPayroll(PayrollReport report, DeductionPayment payment) {
            this.PayrollReport = report;
            this.DeductionPayment = payment;
        }
    }
}
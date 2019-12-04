using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventDeductionPaymentCreated : Event {
        public Deduction Deduction { get; protected set; }
        public DeductionPayment DeductionPayment { get; protected set; }
        public PayrollReport PayrollReport { get; protected set; }

        public EventDeductionPaymentCreated(PayrollReport pr, Deduction d, DeductionPayment dp) {
            PayrollReport = pr;
            Deduction = d;
            DeductionPayment = dp;
        }

        public override string ToString() {
            return $"Deduction Payment created: payroll_report: {PayrollReport.Id}, deduction_id: {Deduction.Id}, amount: {DeductionPayment.PaidAmount}, amort_ratio: {DeductionPayment.PaidAmount / Deduction.AmortizedAmount}";
        }
    }
}
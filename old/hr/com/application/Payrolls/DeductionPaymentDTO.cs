using hr.com.domain.models.Payrolls;

namespace hr.com.application.Payrolls {
    public class DeductionPaymentDTO {
        public string Name { get; protected set; }
        public decimal Amount { get; protected set; }
        
        public DeductionPaymentDTO(DeductionPayment payment) {
            this.Name = payment.DeductionAccount.Name;
            this.Amount = payment.PaidAmount.PreciseValue;
        }
    }
}
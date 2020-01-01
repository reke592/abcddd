using hr.core.domain.shared;

namespace hr.core.domain.Deductions {
    public interface IDeductionPayment {
        long Id { get; }
        MonetaryValue Amount { get; }
    }
}
using hr.core.domain.commons;

namespace hr.core.domain.Deductions {
    public interface IDeductionPayment {
        long Id { get; }
        MonetaryValue Amount { get; }
    }
}
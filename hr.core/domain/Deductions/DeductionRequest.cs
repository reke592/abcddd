using hr.core.domain.shared;

namespace hr.core.domain.Deductions {
    public class DeductionRequest : Entity {
        private long _deduction_id;
        public DeductionType DeductionType { get; protected set; }
        public RequestStatus Status { get; protected set; }
    }
}
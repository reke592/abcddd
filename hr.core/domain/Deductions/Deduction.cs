using System.Collections.Generic;

namespace hr.core.domain.Deductions {
    // Aggregate Root
    public class Deduction : Entity {
        private long _employee_id;
        private AccountName _account;
        private IList<IDeductionPayment> _payments;
        private DeductionRequest _request;
    }
}
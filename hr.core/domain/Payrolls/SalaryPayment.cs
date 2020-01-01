using System.Collections.Generic;
using hr.core.domain.Deductions;
using hr.core.domain.shared;

namespace hr.core.domain.Payrolls {
    // Aggregate Root
    public class SalaryPayment : Entity {
        
        private long _employee_id;
        private long _payroll_period_id;
        public MonetaryValue Gross { get; protected set; }              // component
        public MonetaryValue GrossDeduction { get; protected set; }     // component
        public SalaryPaymentStatus Status { get; protected set; }

        private IList<SalaryDeductionPayment> _deduction_payments;      // 1 - *
    }
}
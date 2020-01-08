using System.Collections.Generic;

namespace hr.core.domain.Payrolls {
    public class PayrollReport : Entity {
        private long _payroll_period_id;
        
        private IList<SalaryPayment> _salaries = new List<SalaryPayment>();     // 1 - *
    }
}
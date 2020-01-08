using System.Collections.Generic;

namespace hr.core.domain.Payrolls {

      public class PayrollPeriod : Entity {
        public virtual int Month { get; protected set; }
        public virtual int Year { get; protected set; }
        public virtual double MonthlyUnit { get; protected set; }

        private IList<PayrollReport> _reports = new List<PayrollReport>();   // 1 to *
        
    }
}
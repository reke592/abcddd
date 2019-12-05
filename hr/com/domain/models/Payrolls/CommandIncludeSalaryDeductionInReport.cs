using hr.com.domain.enums;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandIncludeSalaryDeductionInReport : Command {
        public PayrollReport PayrollReport { get; protected set; }
        public double MonthlyUnit { get; protected set; }

        /// <summary>
        /// default unit = 1 (Unit.WHOLE)
        /// </summary>
        public CommandIncludeSalaryDeductionInReport(PayrollReport report, double unit = Unit.WHOLE) {
            this.PayrollReport = report;
            this.MonthlyUnit = unit;
        }
    }
}
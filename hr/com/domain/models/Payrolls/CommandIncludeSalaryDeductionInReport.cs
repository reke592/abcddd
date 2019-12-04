using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandIncludeSalaryDeductionInReport : Command {
        public PayrollReport PayrollReport { get; protected set; }
        public int Ratio { get; protected set; }

        public CommandIncludeSalaryDeductionInReport(PayrollReport report, int ratio = 1) {
            this.PayrollReport = report;
            this.Ratio = ratio;
        }
    }
}
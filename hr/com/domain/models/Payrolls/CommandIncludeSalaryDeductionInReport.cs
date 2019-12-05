using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandIncludeSalaryDeductionInReport : Command {
        public PayrollReport PayrollReport { get; protected set; }

        public CommandIncludeSalaryDeductionInReport(PayrollReport report) {
            this.PayrollReport = report;
        }
    }
}
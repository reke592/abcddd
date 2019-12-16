using hr.com.domain.models.Employees;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandExcludeDeductionPayment : Command {
        public Employee Employee { get; protected set; }
        public Deduction Deduction { get; protected set; }
        public PayrollReport Report { get; protected set; }

        /// <summary>
        /// Exclude deduction payment on created payroll report
        /// </summary>
        public CommandExcludeDeductionPayment(Employee employee, Deduction deduction, PayrollReport report) {
            this.Employee = employee;
            this.Deduction = deduction;
            this.Report = report;
        }
    }
}
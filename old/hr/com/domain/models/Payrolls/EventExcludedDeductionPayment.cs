using hr.com.domain.models.Employees;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventExcludedDeductionPayment : Event {
        public Employee Employee { get; protected set; }
        public DeductionPayment DeductionPayment { get; protected set; }
        public PayrollReport Report { get; protected set; }

        public EventExcludedDeductionPayment(DeductionPayment payment, Employee employee, PayrollReport report) {
            this.DeductionPayment = payment;
            this.Employee = employee;
            this.Report = report;
        }
    }
}
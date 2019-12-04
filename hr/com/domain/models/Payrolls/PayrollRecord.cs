using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

// TODO: add payroll period, create an aggregate?
namespace hr.com.domain.models.Payrolls {
    // aggregate for deduction payments
    public class PayrollRecord : Entity {
        private PayrollReport _payroll_report;       // reference
        private Salary _salary;                      // reference
        private Employee _employee;                  // reference
        private decimal _gross;                      // copy of current salary.gross value
        private decimal _gross_deduction = 0m;        // updated via CQRS command: IncludeSalaryDeduction
        private IList<DeductionPayment> _deduction_payments = new List<DeductionPayment>();

        public virtual decimal Net {
            get {
                return this._gross - this._gross_deduction;
            }
        }

        public virtual IReadOnlyCollection<DeductionPayment> DeductionPayments {
            get {
                return new ReadOnlyCollection<DeductionPayment>(this._deduction_payments);
            }
        }

        private void onCommandIncludeSalaryDeductionInReport(object sender, Command cmd) {
            if(cmd is CommandIncludeSalaryDeductionInReport) {
                var args = cmd as CommandIncludeSalaryDeductionInReport;
                if(this.Equals(args.PayrollReport)) {
                    foreach(var deduction in this._salary.Deductions) {
                        var payment = DeductionPayment.Create(this._payroll_report, this._employee, deduction);
                        this._deduction_payments.Add(payment);
                        this._gross_deduction += payment.PaidAmount;
                    }
                }
            }
        }

        public PayrollRecord() {
            EventBroker.getInstance().addCommandListener(onCommandIncludeSalaryDeductionInReport);
        }

        public static PayrollRecord Create(PayrollReport pr, Salary sal) {
            return new PayrollRecord {
                _payroll_report = pr,
                _salary = sal,
                _employee = sal.GetEmployee(),

                // copy the gross value to avoid miscalculation in future
                _gross = sal.Gross
            };
        }
    }
}
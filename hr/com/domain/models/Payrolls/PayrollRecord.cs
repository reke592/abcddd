using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

// TODO: add payroll period
namespace hr.com.domain.models.Payrolls {
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

        // TODO: use CQRS Query
        private void onCommandIncludeSalaryDeductionInReport(object sender, Command cmd) {
            if(cmd is CommandIncludeSalaryDeductionInReport) {
                var args = cmd as CommandIncludeSalaryDeductionInReport;
                
                if(args.PayrollReport.Equals(this._payroll_report)) {
                    foreach(var deduction in this._salary.ActiveDeductions) {
                        // because deduction can be whole even if the payroll is half
                        // we include the MonthlyUnit for deduction payment in CQRS command
                        var amount = deduction.AmortizedAmount.PreciseValue * (decimal) args.MonthlyUnit;
                        var payment = DeductionPayment.Create(deduction, MonetaryValue.of(deduction.MonetaryCode, amount));
                        this._deduction_payments.Add(payment);
                        this._gross_deduction += payment.PaidAmount.PreciseValue;

                        EventBroker.getInstance().Emit(new EventDeductionPaymentIncludedInPayroll(this._payroll_report, payment));
                    }
                }
            }
        }

        private void onEventExcludedDeductionPayment(object sender, Event e) {
            if(e is EventExcludedDeductionPayment) {
                var args = e as EventExcludedDeductionPayment;
                if(args.Report.Equals(this._payroll_report) && args.Employee.Equals(this._employee)) {
                    this._deduction_payments.Remove(args.DeductionPayment);
                }
            }
        }

        public PayrollRecord() {
            var broker = EventBroker.getInstance();
            broker.addCommandListener(onCommandIncludeSalaryDeductionInReport);
            broker.addEventListener(onEventExcludedDeductionPayment);
        }

        ~PayrollRecord() {
            var broker = EventBroker.getInstance();
            broker.removeCommandListener(onCommandIncludeSalaryDeductionInReport);
            broker.removeEventListener(onEventExcludedDeductionPayment);
        }

        public static PayrollRecord Create(PayrollReport pr, Salary sal) {
            return new PayrollRecord {
                _payroll_report = pr,
                _salary = sal,
                _employee = sal.GetEmployee(),

                // copy the gross value to avoid miscalculation in future
                _gross = sal.Gross.PreciseValue * (decimal) pr.MonthlyUnit
            };
        }

        public override string ToString() {
            return $"Payroll Report ID: {this._payroll_report.Id}, Record ID: {this.Id}, Gross: {this._gross}, Deduction: {decimal.Round(this._gross_deduction, 3)}, Net: {decimal.Round(this.Net, 3)}";
        }
    }
}
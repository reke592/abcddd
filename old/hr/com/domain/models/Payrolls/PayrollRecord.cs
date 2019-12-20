using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

// TODO: add payroll period
namespace hr.com.domain.models.Payrolls {
    public class PayrollRecord : Entity {
        public virtual PayrollReport ReferencePayrollReport { get; protected set; }       // reference
        public virtual Salary ReferenceSalary { get; protected set; }                      // reference
        public virtual Employee ReferenceEmployee { get; protected set; }                  // reference
        private IList<DeductionPayment> _deduction_payments = new List<DeductionPayment>();
        public virtual decimal Gross { get; protected set; }                      // copy of current salary.gross value
        public virtual decimal GrossDeduction { get; protected set; }        // updated via CQRS command: IncludeSalaryDeduction

        public virtual decimal Net {
            get {
                return this.Gross - this.GrossDeduction;
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
                
                if(args.PayrollReport.Equals(this.ReferencePayrollReport)) {
                    foreach(var deduction in this.ReferenceSalary.ActiveDeductions) {
                        // because deduction can be whole even if the payroll is half
                        // we include the MonthlyUnit for deduction payment in CQRS command
                        // var amount = deduction.AmortizedAmount.PreciseValue * (decimal) args.MonthlyUnit;
                        // var payment = DeductionPayment.Create(deduction, MonetaryValue.of(deduction.MonetaryCode, amount));
                        var payment = deduction.CreatePayment((decimal) args.MonthlyUnit);
                        this._deduction_payments.Add(payment);
                        this.GrossDeduction += payment.PaidAmount.PreciseValue;

                        EventBroker.getInstance().Emit(new EventDeductionPaymentIncludedInPayroll(this.ReferencePayrollReport, payment));
                    }
                }
            }
        }

        private void onEventExcludedDeductionPayment(object sender, Event e) {
            if(e is EventExcludedDeductionPayment) {
                var args = e as EventExcludedDeductionPayment;
                if(args.Report.Equals(this.ReferencePayrollReport) && args.Employee.Equals(this.ReferenceEmployee)) {
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
                ReferencePayrollReport = pr,
                ReferenceSalary = sal,
                ReferenceEmployee = sal.GetEmployee(),

                // copy the gross value to avoid miscalculation in future
                Gross = sal.Gross.PreciseValue * (decimal) pr.MonthlyUnit
            };
        }

        public virtual Person Person {
            get {
                return this.ReferenceEmployee.Person;
            }
        }

        public override string ToString() {
            return $"Payroll Report ID: {this.ReferencePayrollReport.Id}, Record ID: {this.Id}, Gross: {this.Gross}, Deduction: {decimal.Round(this.GrossDeduction, 3)}, Net: {decimal.Round(this.Net, 3)}";
        }
    }
}
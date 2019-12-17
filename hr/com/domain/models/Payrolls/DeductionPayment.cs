using System;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class DeductionPayment : Entity {
        private Employee _employee;                  // reference
        private PayrollReport _payroll_report;       // reference, updated via CQRS event: DeductionPaymentIncludedInPayroll
        private Deduction _deduction;                // reference
        public virtual MonetaryValue PaidAmount { get; protected set; }              // component

        public DeductionAccount DeductionAccount {
            get {
                return this._deduction.Account;
            }
        }

        private void onDeductionPaymentIncludedInPayroll(object sender, Event e) {
            if(e is EventDeductionPaymentIncludedInPayroll) {
                var args = e as EventDeductionPaymentIncludedInPayroll;
                if(args.DeductionPayment.Equals(this)) {
                    this._payroll_report = args.PayrollReport;
                }
            }
        }

        private void onCommandExcludeDeductionPayment(object sender, Command cmd) {
            if(cmd is CommandExcludeDeductionPayment) {
                var args = cmd as CommandExcludeDeductionPayment;
                if(args.Employee.Equals(this._employee) 
                && args.Report.Equals(this._payroll_report)
                && args.Deduction.Equals(this._deduction)) {
                    EventBroker.getInstance().Emit(new EventExcludedDeductionPayment(this, this._employee, this._payroll_report));
                }
            }
        }

        public DeductionPayment() {
            var broker = EventBroker.getInstance();
            broker.addEventListener(onDeductionPaymentIncludedInPayroll);
            broker.addCommandListener(onCommandExcludeDeductionPayment);
        }

        ~DeductionPayment() {
            var broker = EventBroker.getInstance();
            broker.removeEventListener(onDeductionPaymentIncludedInPayroll);
            broker.removeCommandListener(onCommandExcludeDeductionPayment);
        }

        public static DeductionPayment Create(Deduction deduction, MonetaryValue custom_payment = null) {
            
            var record = new DeductionPayment {
                _employee = deduction.GetEmployee(),
                _deduction = deduction,
                // PaidAmount = (custom_payment is null) ? deduction.AmortizedAmount : custom_payment.PreciseValue
                PaidAmount = custom_payment ?? deduction.AmortizedAmount
            };

            // emit event
            EventBroker.getInstance().Emit(new EventDeductionPaymentCreated(record, deduction));

            return record;
        }
    }
}
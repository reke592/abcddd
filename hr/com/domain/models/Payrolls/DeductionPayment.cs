using System;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class DeductionPayment : Entity {
        private Employee _employee;                  // reference
        private PayrollReport _payroll_report;       // reference
        private Deduction _deduction;                // reference
        public virtual decimal PaidAmount { get; protected set; }

         // params:
        //      pr (reference): supplied when deduction payment is computed within a payroll period
        //      employee (reference): in-case the employee want to pay his deduction voluntarily
        public static DeductionPayment Create(PayrollReport report, Employee employee, Deduction deduction, MonetaryValue custom_payment = null) {
            if(report is null && employee is null)
                throw new NullReferenceException("DeductionPayment requires a PayrollReport or Employee reference");
            var record = new DeductionPayment {
                _employee = employee,
                _payroll_report = report,
                _deduction = deduction,
                PaidAmount = (custom_payment is null) ? deduction.AmortizedAmount : custom_payment.PreciseValue
            };

            // emit event
            EventBroker.getInstance().Emit(new EventDeductionPaymentCreated(report, deduction, record));

            return record;
        }
    }
}
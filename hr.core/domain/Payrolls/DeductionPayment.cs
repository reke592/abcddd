using hr.core.domain.shared;

namespace hr.core.domain.Payrolls {
    public class DeductionPayment : Entity {
        // public virtual Employee ReferenceEmployee { get; protected set; }   // reference
        public virtual PayrollReport ReferencePayrollReport { get; protected set; }       // reference, updated via CQRS event: DeductionPaymentIncludedInPayroll
        public virtual Deduction ReferenceDeduction { get; protected set; }                // reference
        public virtual MonetaryValue PaidAmount { get; protected set; }              // component

        // public virtual DeductionAccount DeductionAccount {
        //     get {
        //         return this.ReferenceDeduction.ReferenceAccount;
        //     }
        // }

        // private void onDeductionPaymentIncludedInPayroll(object sender, Event e) {
        //     if(e is EventDeductionPaymentIncludedInPayroll) {
        //         var args = e as EventDeductionPaymentIncludedInPayroll;
        //         if(args.DeductionPayment.Equals(this)) {
        //             this.ReferencePayrollReport = args.PayrollReport;
        //         }
        //     }
        // }

        // private void onCommandExcludeDeductionPayment(object sender, Command cmd) {
        //     if(cmd is CommandExcludeDeductionPayment) {
        //         var args = cmd as CommandExcludeDeductionPayment;
        //         if(args.Employee.Equals(this.ReferenceEmployee) 
        //         && args.Report.Equals(this.ReferencePayrollReport)
        //         && args.Deduction.Equals(this.ReferenceDeduction)) {
        //             EventBroker.getInstance().Emit(new EventExcludedDeductionPayment(this, this.ReferenceEmployee, this.ReferencePayrollReport));
        //         }
        //     }
        // }

        // public DeductionPayment() {
        //     var broker = EventBroker.getInstance();
        //     broker.addEventListener(onDeductionPaymentIncludedInPayroll);
        //     broker.addCommandListener(onCommandExcludeDeductionPayment);
        // }

        // ~DeductionPayment() {
        //     var broker = EventBroker.getInstance();
        //     broker.removeEventListener(onDeductionPaymentIncludedInPayroll);
        //     broker.removeCommandListener(onCommandExcludeDeductionPayment);
        // }

        // public static DeductionPayment Create(Deduction deduction, MonetaryValue payment) {
            
        //     var record = new DeductionPayment {
        //         ReferenceEmployee = deduction.GetEmployee(),
        //         ReferenceDeduction = deduction,
        //         PaidAmount = payment
        //     };

        //     // emit event
        //     EventBroker.getInstance().Emit(new EventDeductionPaymentCreated(record, deduction));

        //     return record;
        // }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.core.domain.shared;

namespace hr.core.domain.Payrolls {
    // aggregate for deduction payments
    public class Deduction : Entity {
        private decimal _paid;  // updated via CQRS Event: DeductionPaymentCreated
        private decimal _amortization;
        private IList<DeductionPayment> _payments = new List<DeductionPayment>();   // 1 to *
        public virtual Salary ReferenceSalary { get; protected set; }   // reference
        public virtual DeductionAccount ReferenceAccount { get; protected set; }     // reference
        // public virtual Employee ReferenceEmployee { get; protected set; }  // reference, update via CQRS Event: SalaryDeductionAdded
        public virtual MonetaryValue Total { get; protected set; }   // component
        public virtual Date DateGranted { get; protected set; }     // component
        public virtual DeductionMode Mode { get; protected set; }

        // private void onEventDeductionPaymentCreated(object sender, Event e) {
        //     if(e is EventDeductionPaymentCreated) {
        //         var args = e as EventDeductionPaymentCreated;
        //         if(args.Deduction.Equals(this)) {
        //             // ratio default: 1
        //             // if(this.hasBalance) {
        //             this._paid += args.DeductionPayment.PaidAmount.PreciseValue / this.AmortizedAmount.PreciseValue;
        //             this._payments.Add(args.DeductionPayment);
        //             // }
        //         }
        //     }
        // }

        // private void onEventSalaryDeductionAdded(object sender, Event e) {
        //     if(e is EventSalaryDeductionAdded) {
        //         var args = e as EventSalaryDeductionAdded;
        //         if(args.Deduction.Equals(this)) {
        //             this.ReferenceEmployee = args.Salary.GetEmployee();
        //         }
        //     }
        // }

        // public Deduction() {
        //     var broker = EventBroker.getInstance();
        //     broker.addEventListener(onEventDeductionPaymentCreated);
        //     broker.addEventListener(onEventSalaryDeductionAdded);
        // }

        // ~Deduction() {
        //     var broker = EventBroker.getInstance();
        //     broker.removeEventListener(onEventDeductionPaymentCreated);
        //     broker.removeEventListener(onEventSalaryDeductionAdded);
        // }

        /// <summary>
        /// Given deduction total
        /// </summary>
        // public static Deduction Create(Salary salary, DeductionAccount account
        //     , int amortization, MonetaryValue total, Date dt_granted = null
        //     , DeductionMode mode = DeductionMode.TEMPORARY) {
        //     var record = new Deduction {
        //         ReferenceSalary = salary
        //         , ReferenceAccount = account
        //         , ReferenceEmployee = salary.GetEmployee()
        //         , Total = total
        //         , _amortization = amortization
        //         , DateGranted = dt_granted ?? Date.TryParse(DateTime.Now.ToLongDateString())
        //         , Mode = mode
        //     };

        //     EventBroker.getInstance().Emit(new EventSalaryDeductionCreated(record, salary));
            
        //     return record;
        // }

        // /// <summary>
        // /// Deduction total = amortized_amount * amortization
        // /// </summary>
        // public static Deduction CreateAmortized(Salary salary, DeductionAccount account
        //     , int amortization, MonetaryValue amortized_amount
        //     , Date dt_granted = null, DeductionMode mode = DeductionMode.TEMPORARY) {
        //     return Deduction.Create(salary
        //         , account
        //         , amortization
        //         , amortized_amount.multipliedBy(amortization)
        //         , dt_granted);
        // }

        // // TODO: fix deduction computation
        // // how about we add a method
        // // DeductionPayment CreatePayment(default decimal unit = 1)
        
        // // this should be in a service
        // public virtual DeductionPayment CreatePayment(decimal unit = 1) {
        //     if(!this.hasBalance)
        //         throw new Exception("Deduction already paid.");

        //     var payment = (this.Total.PreciseValue / this._amortization) * unit;
        //     if(this.Balance.PreciseValue > payment)
        //         return DeductionPayment.Create(this, MonetaryValue.of(this.MonetaryCode, payment));
        //     else
        //         return DeductionPayment.Create(this, this.Balance);
        // }

        public virtual MonetaryValue AmortizedAmount {
            get {
                // if(this.Mode == DeductionMode.CONTINIOUS)
                return this.Total.dividedBy(this._amortization);
                // automatically adjust amortized amount, when custom payment was made
                // bug: when custom payments was made, paid counts eq amortization count
                // return this.Balance.dividedBy(this._amortization - this._payments.Count);
                // fix:
                // var gives = this._amortization - this._payments.Count;
                // return (gives > 1)
                //     ? this.Balance.dividedBy(gives)
                //     : this.Balance;
            }
        }

        public virtual MonetaryValue Paid {
            get {
                return this.Total.dividedBy(this._amortization).multipliedBy(this._paid);
                // return MonetaryValue.of(this.MonetaryCode, (this._total.PreciseValue / this._amortization) * this._paid);
            }
        }

        public virtual MonetaryValue Balance {
            get {
                return this.Total.subtractValueOf(this.Paid);
                // return MonetaryValue.of(this.MonetaryCode, this._total.PreciseValue - this.Paid.PreciseValue);
            }
        }

        public virtual string MonetaryCode {
            get {
                return this.Total.Code;
            }
        }

        public virtual bool hasBalance {
            protected set {}    // required by nhibernate
            get {
                if(this.Mode == DeductionMode.CONTINIOUS)
                    return true;

                return this.Balance.PreciseValue > 0;
            }
        }

        public virtual IReadOnlyCollection<DeductionPayment> Payments {
            get {
                return new ReadOnlyCollection<DeductionPayment>(this._payments);
            }
        }

        // public virtual Employee GetEmployee() {
        //     return this.ReferenceEmployee;
        // }
    }
}
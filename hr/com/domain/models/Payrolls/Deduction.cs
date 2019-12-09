using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    // aggregate for deduction payments
    public class Deduction : Entity {
        private Employee _employee;  // reference, update via CQRS Event: SalaryDeductionAdded
        private Salary _salary;   // reference
        private decimal _paid;  // updated via CQRS Event: DeductionPaymentCreated
        private decimal _amortization;
        private IList<DeductionPayment> _payments = new List<DeductionPayment>();   // 1 to *
        public virtual MonetaryValue Total { get; protected set; }   // component
        public virtual Date DateGranted { get; protected set; }     // component
        public virtual DeductionMode Mode { get; protected set; }

        private void onEventDeductionPaymentCreated(object sender, Event e) {
            if(e is EventDeductionPaymentCreated) {
                var args = e as EventDeductionPaymentCreated;
                if(this.Equals(args.Deduction)) {
                    // ratio default: 1
                    if(this.hasBalance()) {
                        this._paid += args.DeductionPayment.PaidAmount.PreciseValue / this.AmortizedAmount.PreciseValue;
                        this._payments.Add(args.DeductionPayment);
                    }
                }
            }
        }

        private void onEventSalaryDeductionAdded(object sender, Event e) {
            if(e is EventSalaryDeductionAdded) {
                var args = e as EventSalaryDeductionAdded;
                if(this.Equals(args.Deduction)) {
                    this._employee = args.Salary.GetEmployee();
                }
            }
        }

        public Deduction() {
            EventBroker.getInstance().addEventListener(onEventDeductionPaymentCreated);
            EventBroker.getInstance().addEventListener(onEventSalaryDeductionAdded);
        }

        /// <summary>
        /// Given deduction total
        /// </summary>
        public static Deduction Create(Salary salary, int amortization, MonetaryValue total, Date dt_granted = null, DeductionMode mode = DeductionMode.TEMPORARY) {
            var record = new Deduction {
                _salary = salary
                , _employee = salary.GetEmployee()
                , Total = total
                , _amortization = amortization
                , DateGranted = dt_granted ?? Date.TryParse(DateTime.Now.ToLongDateString())
                , Mode = mode
            };

            EventBroker.getInstance().Emit(new EventSalaryDeductionCreated(record, salary));
            
            return record;
        }

        /// <summary>
        /// Deduction total = amortized_amount * amortization
        /// </summary>
        public static Deduction CreateAmortized(Salary salary, int amortization, MonetaryValue amortized_amount, Date dt_granted = null) {
            return Deduction.Create(salary
                , amortization
                , amortized_amount.multipliedBy(amortization)
                , dt_granted);
        }

        public virtual MonetaryValue AmortizedAmount {
            get {
                if(this.Mode == DeductionMode.CONTINIOUS)
                    return this.Total.dividedBy(this._amortization);
                // automatically adjust amortized amount, when custom payment was made
                // return MonetaryValue.of(this.MonetaryCode, this.Balance.PreciseValue / (this._amortization - this._paid));
                // return this.Balance.dividedBy(this._amortization);
                return this.Balance.dividedBy(this._amortization - this._payments.Count);
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

        public virtual bool hasBalance() {
            if(this.Mode == DeductionMode.CONTINIOUS)
                return true;

            return this.Balance.PreciseValue > 0;
        }

        public virtual IReadOnlyCollection<DeductionPayment> Payments {
            get {
                return new ReadOnlyCollection<DeductionPayment>(this._payments);
            }
        }

        public virtual Employee GetEmployee() {
            return this._employee;
        }
    }
}
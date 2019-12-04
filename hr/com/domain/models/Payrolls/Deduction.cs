using System;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class Deduction : Entity {
        private Employee _employee;  // reference, update via CQRS Event: SalaryDeductionAdded
        private decimal _paid;  // updated via CQRS Event: DeductionPaymentCreated
        private decimal _amortization;
        private MonetaryValue _total;   // component
        public virtual Date DateGranted { get; protected set; }     // component

        private void onEventDeductionPaymentCreated(object sender, Event e) {
            if(e is EventDeductionPaymentCreated) {
                var args = e as EventDeductionPaymentCreated;
                if(this.Equals(args.Deduction)) {
                    // ratio default: 1
                    this._paid += args.DeductionPayment.PaidAmount / this.AmortizedAmount;
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

        public static Deduction Create(Salary salary, int amortization, MonetaryValue total, Date dt_granted = null) {
            return new Deduction {
                _employee = salary.GetEmployee(),
                _total = total,
                _amortization = amortization,
                DateGranted = dt_granted ?? Date.TryParse(DateTime.Now.ToLongDateString())
            };
        }

        public virtual decimal AmortizedAmount {
            get {
                return this._total.PreciseValue / this._amortization;
            }
        }

        public virtual decimal TotalAmount {
            get {
                return this._total.PreciseValue;
            }
        }

        public virtual decimal Paid {
            get {
                return AmortizedAmount * this._paid;
            }
        }

        public virtual decimal Balance {
            get {
                return AmortizedAmount * (this._amortization - this._paid);
            }
        }
    }
}
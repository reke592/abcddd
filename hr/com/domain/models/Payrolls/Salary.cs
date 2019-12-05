using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    // aggregate for salary deductions
    public class Salary : Entity {
        private Employee _employee;  // reference
        private MonetaryValue _gross;    // component
        private IList<Deduction> _deductions = new List<Deduction>();    // 1 to *

        // private void onCommandAddSalaryDeduction(object sender, Command cmd) {
        //     if(cmd is CommandAddSalaryDeduction) {
        //         var args = cmd as CommandAddSalaryDeduction;
        //         if(this.Equals(args.Salary)) {
        //             this._deductions.Add(args.Deduction);
        //             EventBroker.getInstance().Emit(new EventSalaryDeductionAdded(this, args.Deduction));
        //         }
        //     }
        // }

        private void onEventSalaryDeductionCreated(object sender, Event e) {
            if(e is EventSalaryDeductionCreated) {
                var args = e as EventSalaryDeductionCreated;
                if(this.Equals(args.Salary)) {
                    this._deductions.Add(args.Deduction);
                    EventBroker.getInstance().Emit(new EventSalaryDeductionAdded(this, args.Deduction));
                }
            }
        }

        public Salary() {
            EventBroker.getInstance().addEventListener(onEventSalaryDeductionCreated);
        }

        public static Salary Create(Employee employee, MonetaryValue gross) {
            var record = new Salary {
                _employee = employee,
                _gross = gross
            };
            EventBroker.getInstance().Command(new CommandAssociateSalaryToEmployee(record, employee));
            return record;
        }

        public virtual decimal Gross {
            get {
                return this._gross.PreciseValue;
            }
        }

        // get only the net value, no persisted updates
        public virtual decimal Net {
            get {
                var net = Gross;
                foreach(Deduction deduction in this._deductions) {
                    net -= deduction.AmortizedAmount;
                }
                return net;
            }
        }

        public virtual IReadOnlyCollection<Deduction> Deductions {
            get {
                return new ReadOnlyCollection<Deduction>(this._deductions);
            }
        }

        // salary.getEmployee() is more readable
        public virtual Employee GetEmployee() {
            return this._employee;
        }
    }
}
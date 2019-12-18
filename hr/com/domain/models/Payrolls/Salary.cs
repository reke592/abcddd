using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    // aggregate for salary deductions
    public class Salary : Entity {
        public virtual Employee ReferenceEmployee { get; protected set; }  // reference
        private IList<Deduction> _deductions = new List<Deduction>();    // 1 to *
        public virtual MonetaryValue Gross { get; protected set; }    // component
        public virtual int YearUpdated { get; protected set; }

        // /// <summary>
        // /// get only the net value against active deductions, no persisted updates
        // /// </summary>
        // public virtual MonetaryValue Net {
        //     get {
        //         var net = Gross.PreciseValue;
        //         foreach(Deduction deduction in this._deductions) {
        //             net -= deduction.hasBalance ? deduction.AmortizedAmount.PreciseValue : 0;
        //         }
        //         return MonetaryValue.of(this.Gross.Code, net);
        //     }
        // }

        // TODO: use CQRS Query instead of eager-loading + iteration in associated deductions
        public virtual IReadOnlyCollection<Deduction> ActiveDeductions {
            get {
                var records = new List<Deduction>();
                foreach(var item in this._deductions) {
                    if(item.hasBalance)
                        records.Add(item);
                }
                return new ReadOnlyCollection<Deduction>(records);
            }
        }

        private void onEventSalaryDeductionCreated(object sender, Event e) {
            if(e is EventSalaryDeductionCreated) {
                var args = e as EventSalaryDeductionCreated;
                if(args.Salary.Equals(this)) {
                    this._deductions.Add(args.Deduction);
                    EventBroker.getInstance().Emit(new EventSalaryDeductionAdded(this, args.Deduction));
                }
            }
        }

        // TODO: unit test
        private void onEventEmployeeSalaryUpdated(object sender, Event e) {
            if(e is EventEmployeeSalaryUpdated) {
                var args = e as EventEmployeeSalaryUpdated;
                if(args.Employee.Equals(this.ReferenceEmployee) && args.Previous != null) {
                    foreach(var deduction in args.Previous.ActiveDeductions) {
                        this._deductions.Add(deduction);
                    }
                }
            }
        }

        public Salary() {
            var broker = EventBroker.getInstance();
            broker.addEventListener(onEventEmployeeSalaryUpdated);
            broker.addEventListener(onEventSalaryDeductionCreated);
        }

        ~Salary() {
            var broker = EventBroker.getInstance();
            broker.removeEventListener(onEventSalaryDeductionCreated);
            broker.removeEventListener(onEventEmployeeSalaryUpdated);
        }

        public static Salary Create(Employee employee, MonetaryValue gross) {
            var record = new Salary {
                ReferenceEmployee = employee,
                Gross = gross,
                YearUpdated = Date.Now.Year
            };

            EventBroker.getInstance().Command(new CommandAssociateSalaryToEmployee(record, employee));

            return record;
        }

        // salary.getEmployee() is more readable
        public virtual Employee GetEmployee() {
            return this.ReferenceEmployee;
        }
    }
}
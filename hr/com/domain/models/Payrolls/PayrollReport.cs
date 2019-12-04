using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.shared;

namespace hr.com.domain.models.Payrolls {
    // aggregate for payroll report
    public class PayrollReport : Entity {
        private IList<PayrollRecord> _records = new List<PayrollRecord>();   // 1 - *
        public virtual int Month { get; protected set; }
        public virtual int Year { get; protected set; }

        public virtual IReadOnlyCollection<PayrollRecord> Records {
            get {
                return new ReadOnlyCollection<PayrollRecord>(this._records);
            }
        }

        public PayrollReport() { }

        private PayrollReport(IList<Salary> salaries, int month, int year) {
            this.Month = month;
            this.Year = year;
            foreach(var salary in salaries) {
                this._records.Add(PayrollRecord.Create(this, salary));
            }
        }

        public static PayrollReport Create(IList<Salary> salaries, Date date) {
            // validation
            return new PayrollReport(salaries, date.Month, date.Year);
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;

namespace hr.com.domain.models.Payrolls {
    // aggregate for payroll record

    // mothly
    // half-month
    // quarter
    // weekly
    // daily
    // should be encapsulated in a factory
    public class PayrollReport : Entity {
        private IList<PayrollRecord> _records = new List<PayrollRecord>();   // 1 to *
        public virtual int Month { get; protected set; }
        public virtual int Year { get; protected set; }
        public virtual double MonthlyUnit { get; protected set; }

        public virtual decimal Total {
            protected set {}    // required by nhibernate
            get {
                var total = 0m;
                foreach(var record in this._records) {
                    total += record.Net;
                }
                return total;
            }
        }

        public virtual IReadOnlyCollection<PayrollRecord> Records {
            get {
                return new ReadOnlyCollection<PayrollRecord>(this._records);
            }
        }

        public PayrollReport() { }

        // TODO: fix time complexity: caused by n+1 queries
        private PayrollReport(IList<Employee> employees, int month, int year, double monthly_unit) {
            this.Month = month;
            this.Year = year;
            this.MonthlyUnit = monthly_unit;
            // we add reference to each PayrollRecord
            foreach(var employee in employees) {
                this._records.Add(PayrollRecord.Create(this, employee.ReferenceSalary));
            }
        }

        /// <summary>
        /// Collects all Salary record, create PayrollRecord for each Salary.
        /// </summary>
        public static PayrollReport Create(IList<Employee> employees, Date date, double monthly_unit = Unit.WHOLE) {
            var record = new PayrollReport(employees, date.Month, date.Year, monthly_unit);
            return record;
        }
    }
}
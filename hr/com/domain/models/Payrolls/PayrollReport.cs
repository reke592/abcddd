using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;

namespace hr.com.domain.models.Payrolls {
        // aggregate for payroll record
    public class PayrollReport : Entity {
        private IList<PayrollRecord> _records = new List<PayrollRecord>();   // 1 to *
        public virtual int Month { get; protected set; }
        public virtual int Year { get; protected set; }
        public virtual double MonthlyUnit { get; protected set; }

        public virtual IReadOnlyCollection<PayrollRecord> Records {
            get {
                return new ReadOnlyCollection<PayrollRecord>(this._records);
            }
        }

        public PayrollReport() { }

        private PayrollReport(IList<Salary> salaries, int month, int year, double monthly_unit) {
            this.Month = month;
            this.Year = year;
            this.MonthlyUnit = monthly_unit;
            // we add reference to each PayrollRecord
            foreach(var salary in salaries) {
                this._records.Add(PayrollRecord.Create(this, salary));
            }
        }

        /// <summary>
        /// Collects all Salary record, create PayrollRecord for each Salary.
        /// </summary>
        public static PayrollReport Create(IList<Employee> employees, Date date, double monthly_unit = Unit.WHOLE) {
            List<Salary> salaries = new List<Salary>();
            foreach(var employee in employees) {
                salaries.Add(employee.GetSalary());
            }
            return new PayrollReport(salaries, date.Month, date.Year, monthly_unit);
        }
    }
}
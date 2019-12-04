using System;
using System.Linq.Expressions;
using hr.com.helper.database;

namespace hr.com.domain.models.Payrolls.specs {
    public class PayrollReportOnSpecificPeriod : Specification<PayrollReport>
    {
        private int _month;
        private int _year;

        public PayrollReportOnSpecificPeriod(int month, int year) {
            this._month = month;
            this._year = year;
        }

        public override Expression<Func<PayrollReport, bool>> toExpression()
        {
            return report => report.Month == this._month && report.Year == this._year;
        }
    }
}
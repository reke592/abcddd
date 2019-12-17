using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.enums;
using hr.com.domain.models.Payrolls;

namespace hr.com.application.Payrolls {
    public class PayrollReportDTO {
        private IList<PayrollRecordDTO> _records = new List<PayrollRecordDTO>();
        public int month { get; protected set; }
        public int year { get; protected set; }
        public string monthly_unit { get; protected set; }
        public IReadOnlyCollection<PayrollRecordDTO> records {
            get {
                return new ReadOnlyCollection<PayrollRecordDTO>(this._records);
            }
        }

        public PayrollReportDTO(PayrollReport report) {
            this.year = report.Year;
            this.month = report.Month;
            this.monthly_unit = Unit.Name(report.MonthlyUnit);
            foreach(var record in report.Records) {
                this._records.Add(new PayrollRecordDTO(record));
            }
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.com.domain.models.Payrolls;

namespace hr.com.application.Payrolls {
    public class PayrollRecordDTO {
        private IList<DeductionPaymentDTO> _deduction_payments = new List<DeductionPaymentDTO>();
        public string first_name { get; protected set; }
        public string middle_name { get; protected set; }
        public string last_name { get; protected set; }
        public string ext_name { get; protected set; }
        public decimal gross { get; protected set; }
        public decimal total_deduction { get; protected set; }
        public IReadOnlyCollection<DeductionPaymentDTO> deduction_payments {
            get {
                return new ReadOnlyCollection<DeductionPaymentDTO>(this._deduction_payments);
            }
        }

        public PayrollRecordDTO(PayrollRecord record) {
            var p = record.Person;
            this.first_name = p.FirstName;
            this.middle_name = p.MiddleName;
            this.last_name = p.LastName;
            this.ext_name = p.ExtName;
            this.gross = record.Gross;
            this.total_deduction = record.GrossDeduction;
            foreach(var payment in record.DeductionPayments) {
                this._deduction_payments.Add(new DeductionPaymentDTO(payment));
            }
        }
    }
}
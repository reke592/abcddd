using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class PayrollRecordMap : ClassMap<PayrollRecord> {
        public PayrollRecordMap() {
            Table("payroll_records");
            Id(x => x.Id);
            References<PayrollReport>(Reveal.Member<PayrollRecord>("_payroll_report"));
            References<Salary>(Reveal.Member<PayrollRecord>("_salary")); // ?
            References<Employee>(Reveal.Member<PayrollRecord>("_employee"));

            Map(Reveal.Member<PayrollRecord>("_gross"));
            Map(Reveal.Member<PayrollRecord>("_gross_deduction"));
            
            HasMany<DeductionPayment>(Reveal.Member<PayrollRecord>("_deduction_payments")).Cascade.All();
        }
    }
}
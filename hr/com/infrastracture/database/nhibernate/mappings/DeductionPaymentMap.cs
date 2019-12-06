using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class DeductionPaymentMap : ClassMap<DeductionPayment> {
        public DeductionPaymentMap() {
            Table("deduction_payments");
            Id(x => x.Id);
            References<Employee>(Reveal.Member<DeductionPayment>("_employee"));
            References<PayrollReport>(Reveal.Member<DeductionPayment>("_payroll_report"));
            References<Deduction>(Reveal.Member<DeductionPayment>("_deduction")).Cascade.SaveUpdate();
            
            // Map(x => x.PaidAmount);
            Component(x => x.PaidAmount, c => {
               c.Map(x => x.Code).Column("money_code");
               c.Map(x => x.PreciseValue).Column("amount");
            }).ColumnPrefix("paid_");
        }
    }
}
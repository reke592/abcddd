using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class DeductionMap : ClassMap<Deduction> {
        public DeductionMap() {
            Table("deductions");
            Id(x => x.Id);
            References<Employee>(Reveal.Member<Deduction>("_employee"));
            References<Salary>(Reveal.Member<Deduction>("_salary"));
            
            Map(Reveal.Member<Deduction>("_amortization"));
            Map(Reveal.Member<Deduction>("_paid"));
            Map(x => x.Mode);

            Component<MonetaryValue>(x => x.Total, c => {
                c.Map(x => x.PreciseValue).Column("total");
                c.Map(x => x.Code);
            }).ColumnPrefix("deduction_");

            Component<Date>(x => x.DateGranted, c => {
                c.Map(x => x.Year);
                c.Map(x => x.Month);
                c.Map(x => x.Day);
            }).ColumnPrefix("granted_");

            HasMany<DeductionPayment>(Reveal.Member<Deduction>("_payments"));
        }
    }
}
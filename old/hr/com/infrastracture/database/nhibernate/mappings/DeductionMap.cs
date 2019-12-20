using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class DeductionMap : ClassMap<Deduction> {
        public DeductionMap() {
            Id(x => x.Id);
            // References<Employee>(Reveal.Member<Deduction>("_employee"));
            References<Employee>(x => x.ReferenceEmployee);
            References<DeductionAccount>(x => x.ReferenceAccount);
            References<Salary>(x => x.ReferenceSalary);
            // References<Salary>(Reveal.Member<Deduction>("_salary"));
        
            Map(Reveal.Member<Deduction>("_amortization"));
            Map(Reveal.Member<Deduction>("_paid"));
            Map(x => x.Mode);
            Map(x => x.hasBalance);

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
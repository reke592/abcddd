using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class SalaryMap : ClassMap<Salary> {
        public SalaryMap() {
            Id(x => x.Id);

            References<Employee>(Reveal.Member<Salary>("_employee")).Cascade.None();

            Component<MonetaryValue>(Reveal.Member<Salary>("_gross"), c => {
                c.Map(x => x.Code);
                c.Map(x => x.PreciseValue).Column("Amount");
            }).ColumnPrefix("gross_");

            HasMany<Deduction>(Reveal.Member<Salary>("_deductions")).Cascade.All();
        }
    }
}
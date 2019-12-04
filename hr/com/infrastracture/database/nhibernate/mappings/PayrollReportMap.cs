using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Payrolls;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class PayrollReportMap : ClassMap<PayrollReport> {
        public PayrollReportMap() {
            Id(x => x.Id);
            Map(x => x.Month);
            Map(x => x.Year);
            HasManyToMany<PayrollRecord>(Reveal.Member<PayrollReport>("_records"));
        }
    }
}
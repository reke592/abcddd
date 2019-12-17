using FluentNHibernate.Mapping;
using hr.com.domain.models.Payrolls;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class DeductionAccountMap : ClassMap<DeductionAccount> {
        public DeductionAccountMap() {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
        }
    }
}
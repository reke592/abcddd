using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.domain.models.Employees;
using hr.domain.shared;

namespace hr.infrastracture.database.nhibernate.mappings.shared {
    public class AddressMap : ClassMap<Address> {
        public AddressMap() {
            Table("Addresses");

            // Id(Reveal.Member<Address>("Id")).GeneratedBy.GuidComb();
            Id(Reveal.Member<Address>("Id")).GeneratedBy.Assigned();
            Map(Reveal.Member<Address>("LotBlock"));
            Map(Reveal.Member<Address>("Street"));
            Map(Reveal.Member<Address>("Municipality"));
            Map(Reveal.Member<Address>("Province"));
            Map(Reveal.Member<Address>("Country")); 

            // for bidirectional <any> relationship?
            // ReferencesAny(Reveal.Member<Address>("reference"))
            //     .AddMetaValue<Employee>(typeof(Employee).Name)
            //     .EntityTypeColumn("table_name")
            //     .EntityIdentifierColumn("table_id")
            //     .IdentityType<long>();
        }
    } 
}
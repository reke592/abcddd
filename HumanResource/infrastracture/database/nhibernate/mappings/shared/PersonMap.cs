using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.domain.shared;

namespace hr.infrastracture.database.nhibernate.mappings.shared {
    public class PersonMap : ClassMap<Person> {
        public PersonMap() {
            Id(Reveal.Member<Person>("Id")).GeneratedBy.Assigned();
            Map(Reveal.Member<Person>("Firstname"));
            Map(Reveal.Member<Person>("Middlename"));
            Map(Reveal.Member<Person>("Surname"));
            Map(Reveal.Member<Person>("Ext"));
            Map(Reveal.Member<Person>("Birthdate"));
            Map(Reveal.Member<Person>("Sex"));
        }
    }
}
using FluentNHibernate.Mapping;
using hr.com.domain.shared;

namespace hr.com.infrastracture.database.nhibernate.mappings.shared {
    public class PersonMap : ClassMap<Person> {
        public PersonMap() {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FirstName);
            Map(x => x.MiddleName);
            Map(x => x.LastName);
            Map(x => x.ExtName);
            Map(x => x.Gender);
            Component<Date>(x => x.Birthdate, c => {
                c.Map(x => x.Year);
                c.Map(x => x.Month);
                c.Map(x => x.Day);
            }).ColumnPrefix("birth_");

            // Id(Reveal.Member<Person>("Id")).GeneratedBy.Assigned();
            // Map(Reveal.Member<Person>("FirstName"));
            // Map(Reveal.Member<Person>("MiddleName"));
            // Map(Reveal.Member<Person>("LastName"));
            // Map(Reveal.Member<Person>("Ext"));
            // Map(Reveal.Member<Person>("Gender"));
            // Component<Date>(Reveal.Member<Person, Date>("Birthdate"), x => {
            //     x.Map(Reveal.Member<Date>("Year"));
            //     x.Map(Reveal.Member<Date>("Month"));
            //     x.Map(Reveal.Member<Date>("Day"));
            // }).ColumnPrefix("birth_");
        }
    }
}
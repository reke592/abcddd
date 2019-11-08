using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.domain.models.Companies;
using hr.domain.models.Employees;
using hr.domain.shared;

namespace hr.infrastracture.database.nhibernate.mappings {
    public class EmployeeMap : ClassMap<Employee> {
        public EmployeeMap() {
            Id(x => x.Id);
            Map(Reveal.Member<Employee>("dateHired"));

            // will add person_id column to employee table (* - 1)
            References<Person>(Reveal.Member<Employee>("personDetails")).Cascade.All();

            // will add department_id column to employee table (1 - 1)
            References<Department>(Reveal.Member<Employee>("department")).Cascade.None();
            // HasOne<Department>(Reveal.Member<Employee>("department"));

            // References<Department>(Reveal.Member<Employee>("department")).Cascade.None();

            // will create separate table to manage m-m relationships between employee and address
            HasManyToMany<Address>(Reveal.Member<Employee>("employeeAddresses"))
                .Table("EmployeeAddresses")
                .Cascade.SaveUpdate();

            // will add all Person property as column to employee table
            // Component<Person>(Reveal.Member<Employee, Person>("personDetails"),
            //     x => {
            //         x.Map(Reveal.Member<Person>("Firstname"));
            //         x.Map(Reveal.Member<Person>("Middlename"));
            //         x.Map(Reveal.Member<Person>("Surname"));
            //         x.Map(Reveal.Member<Person>("Ext"));
            //         x.Map(Reveal.Member<Person>("Birthdate"));
            //         x.Map(Reveal.Member<Person>("Sex"));
            //     }
            // );

            // will add employee_id column to addresses table
            // HasMany<Address>(Reveal.Member<Employee>("employeeAddresses")).Cascade.All();
        }
    }
}
using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;

namespace hr.com.infrastracture.database.nhibernate.mappings {
    public class EmployeeMap : ClassMap<Employee> {
        public EmployeeMap() {
            Id(x => x.Id);
            // will add person_id column to employee table (* - 1)
            // References<Person>(Reveal.Member<Employee>("personDetails")).Cascade.All();
            References<Person>(x => x.Person).Cascade.All();
            // References<Salary>(Reveal.Member<Employee>("_salary")).Cascade.None();
            References<Salary>(x => x.ReferenceSalary).Cascade.None();
            
            Map(x => x.Status);

            Component<Date>(x => x.DateHired, c => {
                c.Map(x => x.Year);
                c.Map(x => x.Month);
                c.Map(x => x.Day);
            }).ColumnPrefix("hired_");

            // Component<Person>(Reveal.Member<Employee, Person>("Person"),
            //     x => {
            //         x.Map(p => p.FirstName);
            //         x.Map(p => p.MiddleName);
            //         x.Map(p => p.LastName);
            //         x.Map(p => p.ExtName);
            //         x.Map(p => p.Gender);
            //         x.Map(p => p.Birthdate);
            //     }
            // ).ColumnPrefix("person_");

            // will add department_id column to employee table (1 - 1)
            // References<Department>(Reveal.Member<Employee>("department")).Cascade.None();
            // HasOne<Department>(Reveal.Member<Employee>("department"));

            // References<Department>(Reveal.Member<Employee>("department")).Cascade.None();

            // will create separate table to manage m-m relationships between employee and address
            // HasManyToMany<Address>(Reveal.Member<Employee>("employeeAddresses"))
            //     .Table("EmployeeAddresses")
            //     .Cascade.SaveUpdate();

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
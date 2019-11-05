using FluentNHibernate;
using FluentNHibernate.Mapping;
using hr.domain.models.Companies;
using hr.domain.models.Employees;

namespace hr.infrastracture.database.nhibernate.mappings {
    public class DepartmentMap : ClassMap<Department> {
        public DepartmentMap() {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Capacity);
            HasMany<Employee>(Reveal.Member<Department>("employees"))
                .Table("department_employees")
                .Cascade.SaveUpdate();
        }
    }
}
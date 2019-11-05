using System;
using hr.domain.models.Employees;
using hr.domain.shared;

namespace hr.infrastracture.database.nhibernate {
    public class Testing {
        public Testing() {
            using(var s = NHibernateHelper.SessionFactory.OpenSession()) {
                using(var t = s.BeginTransaction()) {
                    var e = Employee.Create(Person.Create("Erric John", "Castillo", "Rapsing", "", EnumSex.Male, new DateTime(1992,5,24)));
                    e.addAddress(Address.Create("414", "M.Perez", "Norzagaray", "Bulacan", "Philippines"));
                    s.Save(e);
                    t.Commit();
                }
            }

            using(var s = NHibernateHelper.SessionFactory.OpenSession()) {
                using(var t = s.BeginTransaction()) {
                    var e = s.Get<Employee>(1L);
                    if(!(e.getPersonDetails().Firstname.Equals("Erric John"))) throw new Exception();
                    t.Commit();
                }
            }

            using(var s = NHibernateHelper.SessionFactory.OpenSession()) {
                using(var t = s.BeginTransaction()) {
                    var e = s.Get<Employee>(1L);
                    s.Delete(e);
                    t.Commit();
                }
            }
        }
    }
}
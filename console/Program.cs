using System;
using hr.infrastracture.nhibernate;
using hr.core.models;

namespace console
{
  class Program
  {
    static void Main(string[] args) {
      var x = NHibernateHelper.SessionFactory;
      using (var s = NHibernateHelper.OpenSession()) {
        var data = new PersonDetails();
        data.Rename("Erric John", "Castillo", "Rapsing", null);
        s.SaveOrUpdate(data);
      }
    }
  }
}

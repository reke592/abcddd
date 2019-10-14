using System;
using hr.infrastracture.NHibernate.repositories;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace console
{
  class Program
  {
    static void Main(string[] args) {
      // var x = NHibernateHelper.SessionFactory;
      var repo = new DepartmentRepository();
      Console.WriteLine(repo.All().Count);
    }
  }
}

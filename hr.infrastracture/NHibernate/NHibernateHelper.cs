using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using NHibernate.Tool.hbm2ddl;

namespace hr.infrastracture.NHibernate
{
  public class NHibernateHelper
  {
    private static ISessionFactory _sessionFactory;

    public static ISessionFactory SessionFactory {
      get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
    }

    public static Configuration Configuration { get; set; }

    public static ISession OpenSession() {
      var session = SessionFactory.OpenSession();

      return session;
    }

    public static IStatelessSession OpenStatelessSession() {
      var session = SessionFactory.OpenStatelessSession();

      return session;
    }

    private static ISessionFactory CreateSessionFactory() {
      if (Configuration == null) {
        var props = new Dictionary<string, string>();
        props.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
        props.Add("connection.connection_string", "Data Source=test.db;Pooling=true;Version=3");
        props.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
        props.Add("show_sql", "true");
        Configuration = new Configuration().AddProperties(props).AddAssembly("hr.infrastracture");
        new SchemaUpdate(Configuration).Execute(true, true);
      }
      var sessionFactory = Configuration.BuildSessionFactory();
      return sessionFactory;
    }
  }
}
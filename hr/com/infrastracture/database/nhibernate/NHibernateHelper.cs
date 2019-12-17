using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using hr.com.helper.domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace hr.com.infrastracture.database.nhibernate {
    public class NHibernateHelper : IUnitOfWorkProvider<NHUnitOfWork> {
        private static ISessionFactory _sessionFactory;
       
        public static ISessionFactory SessionFactory {
            get => _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
        }

        private static ISessionFactory CreateSessionFactory() {
            // if(Configuration == null) {
            //     var props = new Dictionary<string, string>();
            //     props.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            //     props.Add("connection.connection_string", "Data Source=test.db;Pooling=true;Version=3");
            //     props.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            //     props.Add("show_sql", "true");
            //     Configuration = new Configuration()
            //         .AddProperties(props)
            //         .AddAssembly(typeof(NHibernateHelper).Assembly);
            // }
            return Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("test.db").ShowSql())
                .CurrentSessionContext("thread_static")
                .Mappings(m => {
                    m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>();
                })
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration cfg) {
            if(File.Exists("test.db")) 
                File.Delete("test.db");
            
            new SchemaExport(cfg).Create(true, true);
        }

        /// <summary>
        /// Bind the SessionFactory to CurrentSessionContext and return the current session.
        /// </summary>
        public static ISession GetCurrentSession() {
            if(!CurrentSessionContext.HasBind(SessionFactory)) {
                CurrentSessionContext.Bind(SessionFactory.OpenSession());
            }
            return SessionFactory.GetCurrentSession();
        }

        /// <summary>
        /// Unbind the SessionFactory to CurrentSessionContext, close and dispose the current session.
        /// </summary>
        public static void DisposeCurrentSession() {
            ISession currentSession = CurrentSessionContext.Unbind(SessionFactory);
            currentSession.Close();
            currentSession.Dispose();
        }

        public NHUnitOfWork CreateTransaction()
        {
            return new NHUnitOfWork();
        }
    }
}
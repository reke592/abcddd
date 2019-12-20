using System;
using NHibernate;
using hr.com.helper.database;
using System.Threading.Tasks;

namespace hr.com.infrastracture.database.nhibernate {
    public class NHUnitOfWork : IUnitOfWork
    {
        public ITransaction Transaction;
        public ISession Session { get; private set; }
        public IStatelessSession StatelessSession { get; private set; }
        public bool isStateless { get; private set; }

        private NHUnitOfWork() { }

        public static NHUnitOfWork Statefull {
            get {
                var session = NHibernateHelper.GetCurrentSession();
                return new NHUnitOfWork {
                    isStateless = false
                    , Session = session
                    , Transaction = session.BeginTransaction()
                };
            }
        }

        public static NHUnitOfWork Stateless {
            get {
                var session = NHibernateHelper.SessionFactory.OpenStatelessSession();
                return new NHUnitOfWork {
                    isStateless = true
                    , StatelessSession = session
                    , Transaction = session.BeginTransaction()
                };
            }
        }

        public void Dispose()
        {
            if(this.isStateless)
            {
                StatelessSession.Close();
                StatelessSession.Dispose();
            }
            // unbind the context
            // see: NHibernateHelper.cs
            else
            {
                NHibernateHelper.DisposeCurrentSession();
            }
        }

        public void Commit()
        {
            Transaction.Commit();
        }
    }
}
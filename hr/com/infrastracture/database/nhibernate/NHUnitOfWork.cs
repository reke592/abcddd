using System;
using NHibernate;
using hr.com.helper.database;

namespace hr.com.infrastracture.database.nhibernate {
    public class NHUnitOfWork : IUnitOfWork, IDisposable
    {
        private ITransaction transaction;
        public ISession Session { get; private set; }

        public NHUnitOfWork() {
            // bind the context
            // see: NHibernateHelper.cs
            Session = NHibernateHelper.GetCurrentSession();
            transaction = Session.BeginTransaction();
        }

        public void Dispose()
        {
            // unbind the context
            // see: NHibernateHelper.cs
            NHibernateHelper.DisposeCurrentSession();
        }

        public void Commit()
        {
            transaction.Commit();
        }
    }
}
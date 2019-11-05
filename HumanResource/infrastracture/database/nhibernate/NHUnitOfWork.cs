using System;
using hr.helper.database;
using NHibernate;

namespace hr.infrastracture.database.nhibernate {
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
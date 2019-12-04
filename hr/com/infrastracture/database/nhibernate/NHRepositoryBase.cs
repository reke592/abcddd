using System.Collections.Generic;
using hr.com.helper.database;
using System.Linq;

namespace hr.com.infrastracture.database.nhibernate {
    // NOTE:
    // application will begin a transaction
    // we use the current session
    // we commit the transaction in application layer iff no errors thrown in domain services
    public abstract class NHRepositoryBase<T> : IRepository<T>
    {
        // TODO:
        // support specification pattern
        public T Find(Specification<T> spec)
        {
            var s = NHibernateHelper.GetCurrentSession();
            return s.Query<T>().Where(spec.toExpression()).FirstOrDefault();
        }

        public IList<T> FindAll(Specification<T> spec)
        {
            var s = NHibernateHelper.GetCurrentSession();
            return s.Query<T>().Where(spec.toExpression()).ToList();
        }

        public T Save(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Save(obj);
            return obj;
        }

        public void Update(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Update(obj);
        }

        public void SaveOrUpdate(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.SaveOrUpdate(obj);
        }

        public void Delete(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Delete(obj);
        }
    }
}
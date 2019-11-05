using NHibernate;

namespace hr.infrastracture.database.nhibernate {
    public abstract class NHRepositoryBase<T> : helper.database.IRepository<T>
    {
        // TODO:
        // application will begin a transaction
        // we use the current session
        // we commit the transaction in application layer iff no errors thrown in domain services
        public void delete(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Delete(obj);
        }

        public T save(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Save(obj);
            return obj;
        }

        public void update(T obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Update(obj);
        }
    }
}
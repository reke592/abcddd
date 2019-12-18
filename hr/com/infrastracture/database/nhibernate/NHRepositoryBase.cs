using System;
using System.Linq;
using System.Collections.Generic;
using hr.com.helper.database;
using System.Threading.Tasks;
using hr.com.domain;

namespace hr.com.infrastracture.database.nhibernate {
    // NOTE:
    // application will begin a transaction
    // we use the current session
    // we commit the transaction in application layer iff no errors thrown in domain services
    public abstract class NHRepositoryBase<TEntity> : IRepository<TEntity>
    {
        // TODO:
        // support specification pattern
        public TEntity Find(Specification<TEntity> spec, bool err_notfound = false)
        {
            var s = NHibernateHelper.GetCurrentSession();
            var record = s.Query<TEntity>().Where(spec.toExpression()).FirstOrDefault();
            if(err_notfound && record == null)
                throw new Exception($"{typeof(TEntity).Name} record not found.");
            else
                return record;
        }

        public IList<TEntity> FindAll(Specification<TEntity> spec, bool err_notfound = false)
        {
            var s = NHibernateHelper.GetCurrentSession();
            var records = s.Query<TEntity>().Where(spec.toExpression()).ToList();
            if(err_notfound && records.Count == 0)
                throw new Exception($"{typeof(TEntity).Name} records not found.");
            else
                return records;
        }

        // generics
        public TOther Find<TOther>(Specification<TOther> spec) {
            var s = NHibernateHelper.GetCurrentSession();
            var record = s.Query<TOther>().Where(spec.toExpression()).FirstOrDefault();
            return record;
        }

        public IList<TOther> FindAll<TOther>(Specification<TOther> spec) {
            var s = NHibernateHelper.GetCurrentSession();
            var records = s.Query<TOther>().Where(spec.toExpression()).ToList();
            return records;
        }

        public IList<T> Query<T>(string sql)
        {
            var s = NHibernateHelper.GetCurrentSession();
            return s.CreateQuery(sql).List<T>();
        }

        public TEntity Save(TEntity obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Save(obj);
            return obj;
        }

        public void Update(TEntity obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Update(obj);
        }

        public void SaveOrUpdate(TEntity obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.SaveOrUpdate(obj);
        }

        public void Delete(TEntity obj)
        {
            var s = NHibernateHelper.GetCurrentSession();
            s.Delete(obj);
        }
    }
}
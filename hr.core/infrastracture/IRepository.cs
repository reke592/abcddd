using System.Collections.Generic;
using System.Threading.Tasks;

namespace hr.core.infrastracture {
    public interface IRepository<TEntity> {
        TEntity Find(Specification<TEntity> spec, bool err_notfound = false);
        IList<TEntity> FindAll(Specification<TEntity> spec, bool err_notfound = false);

        // generics
        T Find<T>(Specification<T> spec);
        IList<T> FindAll<T>(Specification<T> spec);
        IList<T> Query<T>(string sql);

        TEntity Save(TEntity obj);
        void Update(TEntity obj);
        void SaveOrUpdate(TEntity obj);
        void Delete(TEntity obj);
    }
}
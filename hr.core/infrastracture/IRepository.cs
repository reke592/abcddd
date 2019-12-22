using System.Collections.Generic;

namespace hr.core.infrastracture {
    public interface IRepository<TEntity> {
        // for mid-life persistence
        long NextId { get; }

        // transient processing, mid-life
        void Add(TEntity obj);
        bool Remove(TEntity obj);
        TEntity Find(Specification<TEntity> spec, bool err_notfound = false);
        IList<TEntity> FindAll(Specification<TEntity> spec, bool err_notfound = false);

        // persistence, end-life
        TEntity Save(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);

        // generics
        // T Find<T>(Specification<T> spec);
        // IList<T> FindAll<T>(Specification<T> spec);
        // IList<T> Query<T>(string sql);
        // void SaveOrUpdate(TEntity obj);
    }
}
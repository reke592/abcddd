using System.Collections.Generic;

namespace hr.com.helper.database {
    public interface IRepository<TEntity> {
        TEntity Find(Specification<TEntity> spec);
        IList<TEntity> FindAll(Specification<TEntity> spec);
        TEntity Save(TEntity obj);
        void Update(TEntity obj);
        void SaveOrUpdate(TEntity obj);
        void Delete(TEntity obj);
    }
}
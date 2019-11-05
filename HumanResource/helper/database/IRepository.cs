namespace hr.helper.database {
    public interface IRepository<TEntity> {
        TEntity Save(TEntity obj);
        void Update(TEntity obj);
        void SaveOrUpdate(TEntity obj);
        void Delete(TEntity obj);
    }
}
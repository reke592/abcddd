namespace hr.helper.database {
    public interface IRepository<TEntity> {
        TEntity save(TEntity obj);
        void update(TEntity obj);
        void delete(TEntity obj);
    }
}
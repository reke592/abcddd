using hr.com.helper.database;

namespace hr.com.helper.domain {
    public interface IUnitOfWorkProvider<T> where T : IUnitOfWork {
        T CreateTransaction();
    }
}
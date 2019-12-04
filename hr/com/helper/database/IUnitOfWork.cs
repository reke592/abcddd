using System;

namespace hr.com.helper.database {
    public interface IUnitOfWork : IDisposable {
        void Commit();
    }
}
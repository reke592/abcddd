using System;

namespace hr.core.infrastracture {
    public interface IUnitOfWork : IDisposable {
        void Commit();
    }
}
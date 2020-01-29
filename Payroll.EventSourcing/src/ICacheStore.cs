using System;
using System.Collections.Generic;

namespace Payroll.EventSourcing
{
    public interface ICacheStore
    {
        // T is a document projection snapshot
        void UpdateIfFound<T>(Guid id, Action<T> apply);

        // store document in memory
        void Store<T>(Guid id, T document);

        // delete document in memory
        void Delete<T>(Guid id);

        // delete all document in memory
        void DeleteAll<T>();

        // returns projection document
        T Get<T>(Guid id);

        // returns all projection document
        IReadOnlyCollection<T> All<T>();
    }
}
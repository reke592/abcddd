using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hris.xunit.units.EventSourcing
{
    public interface ISnapshotStore
    {
        // T is a document projection snapshot
        void UpdateIfFound<T>(Guid id, Action<T> apply);

        // store document in memory
        void Store<T>(Guid id, T document);

        // delete document in memory
        void Delete<T>(Guid id);

        // returns projection document
        T Get<T>(Guid id);

        // returns all projection document
        IReadOnlyCollection<T> All<T>();
    }
}
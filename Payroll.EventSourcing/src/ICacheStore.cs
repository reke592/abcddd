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

        // returns projection document
        T Get<T>(Guid id);

        // returns all projection document
        IReadOnlyCollection<T> All<T>();

        // apply same updates to all record, DO NOT use in golded copies
        // void NukeUpdate<T>(Action<T> update);

        // check if snapshots contains any T record, used to verify single record
        // bool ContainsAny<T>();
    }
}
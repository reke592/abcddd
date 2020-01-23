using System;
using hris.xunit.units.domain;

namespace hris.xunit.units.EventSourcing
{
    public interface IEventStore
    {
        // persist changes, 1 change = +1 version
        void Save<T>(T record) where T : Aggregate;

        // returns the whole event stream related to aggregate
        object[] Get<T>(Guid id) where T : Aggregate;

        // return all events in stream to project the previous version of an aggregate
        object[] GetPreviousVersion<T>(Guid id, int versionOffset = 1);

        // returns the current most current version of the aggregate
        long LatestVersion<T>(T record) where T : Aggregate;

        void AfterSave(EventHandler<object[]> handler);

        void AfterDBReload(EventHandler<object[]> handler);
    }
}
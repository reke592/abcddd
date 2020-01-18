using System;

namespace hris.xunit.units.EventSourcing
{
    public interface IProjection
    {
        // 1 projection = 1 subscription
        void Handle(object e, ISnapshotStore snapshots);
    }
}
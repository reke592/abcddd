using System;

namespace hris.xunit.units.EventSourcing
{
    public interface IProjection
    {
        void Handle(object e, ISnapshotStore snapshots);
    }
}
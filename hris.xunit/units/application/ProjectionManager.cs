using System;
using System.Collections.Generic;
using hris.xunit.units.domain;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units.application
{
    public class ProjectionManager : IProjectionManager
    {
        private ISnapshotStore _snapshots;
        private ISet<IProjection> _projections;
        
        public ProjectionManager(ISnapshotStore store)
        {
            _snapshots = store;
             _projections = new HashSet<IProjection>();
        }

        public void Register(IProjection projection)
        {
            _projections.Add(projection);
        }

        public void UpdateProjections(object sender, object[] events)
        {
            foreach(var projection in _projections)
                foreach(var e in events)
                    projection.Handle(e, _snapshots);
        }
    }
}
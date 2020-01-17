using System;
using hris.xunit.units.domain;

namespace hris.xunit.units.EventSourcing
{
    public interface IProjectionManager
    {
        void UpdateProjections(object sender, object[] events);
        void Register(IProjection projection);
        T GetProjection<T>(Guid id) where T : IProjection;
    }
}
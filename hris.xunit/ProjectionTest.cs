using System;
using System.Collections.Generic;
using hris.xunit.units;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;
using hris.xunit.units.Serialization;
using Xunit;

namespace hris.xunit
{
    public abstract class ProjectionTestBase : IDisposable
    {
        protected ITypeMapper _mapper;
        protected IEventStore _db_events;
        protected ISnapshotStore _db_snapshots;
        protected IProjectionManager _projections;

        public ProjectionTestBase()
        {
            _mapper = new TypeMapper();
            _mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            _mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            _mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            _mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            _mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");

            _db_events = new MemoryEventStore(_mapper);
            _db_snapshots = new MemorySnapshotStore();
            _projections = new ProjectionManager(_db_snapshots);
            _projections.Register(new ActiveEmployeesProjection());
            _db_events.AfterSave(_projections.UpdateProjections);
        }

        public void Dispose()
        {
            _mapper = null;
            _db_events = null;
            _db_snapshots = null;
            _projections = null;
        }
    }

    public class ProjectionTest : ProjectionTestBase
    {
        [Fact]
        public void CanCreateProjections()
        {          
            var id = new EmployeeId(Guid.NewGuid());
            var stubEmployee = Employee.Create(id, DateTimeOffset.Now);
            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);
            stubEmployee.setActive(DateTimeOffset.Now);
            _db_events.Save(stubEmployee);

            var id2 = new EmployeeId(Guid.NewGuid());
            var stubEmployee2 = Employee.Create(id2, DateTimeOffset.Now);
            stubEmployee2.updateBio(new Bio("camilla", "", "dela torre", "1/1/2000"), DateTimeOffset.Now);
            _db_events.Save(stubEmployee2);

            var actual = _db_snapshots.All<ActiveEmployeeDocument>();
            Assert.Equal(1, actual.Count);
        }

        [Fact]
        public void CanProjectPreviousVersion()
        {
            var id = Guid.NewGuid();
            var stubEmployee = Employee.Create(id, DateTimeOffset.Now);
            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);     // v1
            _db_events.Save(stubEmployee);

            var events = _db_events.Get<Employee>(id);
            var next = new Employee();
            next.Load(events);

            next.updateBio(new Bio("camilla", "", "dela torre", "1/1/2000"), DateTimeOffset.Now);               // v2
            next.setActive(DateTimeOffset.Now);                                                                 // v3
            next.setInactive(DateTimeOffset.Now);                                                               // v4
            _db_events.Save(next);

            var previous = _db_events.GetPreviousVersion<Employee>(stubEmployee.Id, 3);
            var actual = new Employee();
            actual.Load(previous);

            Assert.Equal(id, actual.Id);
            Assert.Equal(1, actual.Version);
            Assert.Equal("juan", actual.Bio.FirstName);
        }
    }
}
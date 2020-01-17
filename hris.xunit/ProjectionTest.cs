using System;
using hris.xunit.units.application;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;
using Xunit;

namespace hris.xunit
{
    public class ProjectionTest
    {
        [Fact]
        public void CanCreateProjections()
        {
            var mapper = new TypeMapper();
            mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");
            var event_store = new MemoryEventStore(mapper);
            var snapshot_store = new MemorySnapshotStore();
            var projections = new ProjectionManager(snapshot_store);
            projections.Register(new ActiveEmployeesProjection());
            event_store.afterSave += projections.UpdateProjections;
            
            var id = new EmployeeId(Guid.NewGuid());
            var stubEmployee = Employee.Create(id, DateTimeOffset.Now);
            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);
            stubEmployee.setActive(DateTimeOffset.Now);
            event_store.Save(stubEmployee);

            var id2 = new EmployeeId(Guid.NewGuid());
            var stubEmployee2 = Employee.Create(id2, DateTimeOffset.Now);
            stubEmployee2.updateBio(new Bio("camilla", "", "dela torre", "1/1/2000"), DateTimeOffset.Now);
            event_store.Save(stubEmployee2);

            var actual = snapshot_store.All<ActiveEmployeeDocument>();
            Assert.Equal(1, actual.Count);
        }
    }
}
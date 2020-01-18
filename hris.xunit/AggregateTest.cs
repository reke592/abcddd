using System;
using hris.xunit.units;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;
using Xunit;

namespace hris.xunit {
    public class AggregateTest
    {
        [Fact]
        public void AggregateCanBeLoadedFromEventObjects()
        {
            var stubEmployee = Employee.Create(Guid.NewGuid(), DateTimeOffset.Now);
            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);
            
            var events = stubEmployee.Events;

            var actual = new Employee();
            actual.Load(events);

            Assert.Equal(actual.Bio, stubEmployee.Bio);
            Assert.NotSame(stubEmployee, actual);
        }

        [Fact]
        public void EventStoreCanSaveAggregateUpdates()
        {
            var mapper = new TypeMapper();
            var store = new MemoryEventStore(mapper);
            var stubEmployee = Employee.Create(Guid.NewGuid(), DateTimeOffset.Now);
            mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");

            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);
            store.Save<Employee>(stubEmployee);
            
            var actual = store.LatestVersion(stubEmployee);

            Assert.Equal(actual, 1);
        }

        [Fact]
        public void CanRebuildAggregateUsingEventsInEventStore()
        {
            var mapper = new TypeMapper();
            var store = new MemoryEventStore(mapper);
            var id = new EmployeeId(Guid.NewGuid());
            var stubEmployee = Employee.Create(id, DateTimeOffset.Now);
            mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");

            stubEmployee.updateBio(new Bio("juan", "santos", "dela cruz", "1/1/2000"), DateTimeOffset.Now);
            store.Save<Employee>(stubEmployee);
            var events = store.Get<Employee>(id);

            var actual = new Employee();
            actual.Load(events);

            Assert.Equal(1, actual.Version);
            Assert.Equal(stubEmployee.Bio.DateOfBirth, actual.Bio.DateOfBirth);
            Assert.NotSame(stubEmployee, actual);
        }
    }
}
using System;
using hris.xunit.units;
using hris.xunit.units.application.Employees;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;
using Xunit;

namespace hris.xunit
{
    public abstract class TestBase : IDisposable
    {
        protected IEmployeeAppService _emp_app;
        
        public TestBase()
        {
            var mapper = new TypeMapper();
            var event_store = new MemoryEventStore(mapper);
            var snapshot_store = new MemorySnapshotStore();
            var projection_manager = new ProjectionManager(snapshot_store);
            // register event type to event string
            mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");
            // register projection for active employees
            projection_manager.Register(new ActiveEmployeesProjection());
            // update projections when store updated
            event_store.afterSave += projection_manager.UpdateProjections;
            // create app service
            _emp_app = new EmployeeAppService(event_store, snapshot_store);
        }

        public void Dispose()
        {
            _emp_app = null;
        }
    }


    public class EmployeeAppServiceTest : TestBase
    {
        [Fact]
        public void CanAddEmployeeUsingCommand()
        {
            var new_record_id = _emp_app.AddEmployee(new Contracts.V1.CreateEmployeeCommand {
                FirstName = "Juan",
                MiddleName = "Santos",
                LastName = "Dela Cruz",
                DateOfBirth = "1/1/2001",
                CreatedAt = DateTimeOffset.Now.ToString()
            });

            var actual = _emp_app.ActivateEmployee(new Contracts.V1.ActivateEmployeeCommand {
                Id = new_record_id.ToString(),
                ActivatedAt = DateTimeOffset.Now.ToString()
            });

            Assert.Equal(EmployeeStatus.ACTIVE, actual.Status);
            Assert.Equal("Juan", actual.Bio.FirstName);
        }
    }
}
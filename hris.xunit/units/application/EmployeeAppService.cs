using System;
using System.Collections.Generic;
using System.Linq;
using hris.xunit.units.application.Employees;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units.application
{
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEventStore _store;
        private readonly ISnapshotStore _snapshots;

        // constructor
        public EmployeeAppService(IEventStore store, ISnapshotStore snapshots)
        {
            _store = store;
            _snapshots = snapshots;
        }

        public List<ActiveEmployeeDocument> GetActiveEmployees()
            => _snapshots.All<ActiveEmployeeDocument>().ToList();

        public EmployeeId AddEmployee(Contracts.V1.CreateEmployeeCommand command)
        {
            var Id = new EmployeeId(Guid.NewGuid());
            var ee = Employee.Create(Id, DateTimeOffset.Parse(command.CreatedAt));
            ee.updateBio(
                new Bio(command.FirstName, command.MiddleName, command.LastName, command.DateOfBirth)
                , DateTimeOffset.Parse(command.CreatedAt));
            _store.Save<Employee>(ee);
            return Id;
        }

        public ActiveEmployeeDocument ActivateEmployee(Contracts.V1.ActivateEmployeeCommand command)
        {
            var Id = Guid.Parse(command.Id);
            var events = _store.Get<Employee>(Id);
            var ee = new Employee();
            ee.Load(events);
            ee.setActive(DateTimeOffset.Parse(command.ActivatedAt));
            _store.Save(ee);
            return _snapshots.Get<ActiveEmployeeDocument>(Id);
        }
    }
}
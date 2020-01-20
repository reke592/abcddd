using System;
using hris.xunit.units;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;
using hris.xunit.units.EventSourcing;
using hris.xunit.units.Serialization;
using Xunit;

namespace hris.xunit
{
    public abstract class YAMLSerializationTestBase : IDisposable
    {
        protected ITypeMapper _mapper;
        protected IEventStore _db_events;
        protected ISnapshotStore _db_snapshots;
        protected IProjectionManager _projections;
        protected IYAMLSerializer _serializer;

        public YAMLSerializationTestBase()
        {
            _mapper = new TypeMapper();
            _mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            _mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            _mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            _mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            _mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");

            _serializer = new YAMLSerializer(_mapper);
            _db_events = new MemoryEventStore(_mapper, _serializer);
            _db_snapshots = new MemorySnapshotStore();
            _projections = new ProjectionManager(_db_snapshots);
            _projections.Register(new ActiveEmployeesProjection());
            _db_events.AfterSave(_projections.UpdateProjections);
        }

        protected Guid CreateStub(Bio bio, bool active = false)
        {
            var id = Guid.NewGuid();
            var stub = Employee.Create(id, DateTimeOffset.Now);
            stub.updateBio(bio, DateTimeOffset.Now);
            if(active) stub.setActive(DateTimeOffset.Now);
            _db_events.Save(stub);
            return id;
        }

        public void Dispose()
        {
            _mapper = null;
            _db_events = null;
            _db_snapshots = null;
            _projections = null;
        }
    }

    public class YAMLSerializationTest : YAMLSerializationTestBase
    {
        [Fact]
        public void CanSerializeDeserializeEventStoreToYAML()
        {
            var stub1 = CreateStub(new Bio("juan", "santos", "dela cruz", "1/1/2000"));
            var stub2 = CreateStub(new Bio("camilla", "", "dela torre", "2/2/2000"), active: true);
            var stub3 = CreateStub(new Bio("juan", "", "felipe", "3/3/2000"));
            var db = _db_events as IYAMLSerializable;
            var raw_string = db.YAML_Export();
            
            var test_load = new MemoryEventStore(_mapper, _serializer);
            var actual = new Employee();
            test_load.YAML_Load(raw_string);
            actual.Load(test_load.Get<Employee>(stub2));

            Assert.Equal("camilla", actual.Bio.FirstName);
            Assert.Equal(2, actual.Version);
        }
    }
}
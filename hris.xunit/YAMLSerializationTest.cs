using System;
using hris.xunit.units;
using hris.xunit.units.EventSourcing;
using hris.xunit.units.Serialization;
using hris.xunit.units.domain.Employees;
using Xunit;

namespace hris.xunit
{
    public abstract class YAMLSerializationTestBase : IDisposable
    {
        
        protected ITypeMapper _v1_mapper;
        protected ITypeMapper _v2_mapper;

        public YAMLSerializationTestBase()
        {
            _v1_mapper = new TypeMapper();
            _v1_mapper.Map<Events.V1.EmployeeActivated>("EmployeeActivated");
            _v1_mapper.Map<Events.V1.EmployeeBioUpdated>("EmployeeBioUpdated");
            _v1_mapper.Map<Events.V1.EmployeeCreated>("EmployeeCreated");
            _v1_mapper.Map<Events.V1.EmployeeDeactivated>("EmployeeDeactivated");
            _v1_mapper.Map<Events.V1.EmployeeLeaveGranted>("EmployeeLeaveGranted");

            _v2_mapper = new TypeMapper();
            _v2_mapper.Map<Events.V2.EmployeeBioUpdated>("EmployeeBioUpdated");
            _v2_mapper.Map<Events.V2.EmployeeCreated>("EmployeeCreated");
            _v2_mapper.Map<Events.V2.EmployeeActivated>("EmployeeActivated");
            _v2_mapper.Map<Events.V2.EmployeeDeactivated>("EmployeeDeactivated");
            _v2_mapper.Map<Events.V2.EmployeeLeaveGranted>("EmployeeLeaveGranted");
        }

        protected Guid CreateStubV1(IEventStore _db_events, Bio bio, bool active = false)
        {
            var id = Guid.NewGuid();
            var stub = units.domain.Employees.Employee.Create(id, DateTimeOffset.Now);
            stub.updateBio(bio, DateTimeOffset.Now);
            if(active) stub.setActive(DateTimeOffset.Now);
            _db_events.Save(stub);
            return id;
        }

        protected Guid[] CreateStubV2(IEventStore _db_events, Bio bio, bool active = false)
        {
            var owner = Guid.NewGuid();
            var id = Guid.NewGuid();
            var stub = units.v2.domain.Employees.Employee.Create(id, owner, DateTimeOffset.Now);
            stub.updateBio(bio, owner, DateTimeOffset.Now);
            if(active) stub.setActive(owner, DateTimeOffset.Now);
            _db_events.Save(stub);
            return new Guid[] { owner, id };
        }

        public void Dispose()
        {
            _v1_mapper = null;
            _v2_mapper = null;
        }
    }

    public class YAMLSerializationTest : YAMLSerializationTestBase
    {
        [Fact]
        public void CanSerializeDeserializeEventStoreToYAML()
        {
            var _serializer = new YAMLSerializer(_v1_mapper);
            var _db_events = new MemoryEventStore(_v1_mapper, _serializer);
            var _db_snapshots = new MemorySnapshotStore();
            var _projections = new ProjectionManager(_db_snapshots);
            _projections.Register(new units.application.Employees.Projections.ActiveEmployeesProjection());
            _db_events.AfterSave(_projections.UpdateProjections);
            
            var stub1 = CreateStubV1(_db_events,
                new Bio("juan", "santos", "dela cruz", "1/1/2000"));
            var stub2 = CreateStubV1(_db_events,
                new Bio("camilla", "", "dela torre", "2/2/2000"), active: true);
            var stub3 = CreateStubV1(_db_events,
                new Bio("juan", "", "felipe", "3/3/2000"));
            
            var db = _db_events as IYAMLSerializable;
            var raw_string = db.YAML_Export();
            
            var test_load = new MemoryEventStore(_v1_mapper, _serializer);
            var actual = new units.domain.Employees.Employee();
            test_load.YAML_Load(raw_string);
            actual.Load(test_load.Get<units.domain.Employees.Employee>(stub2));

            Assert.Equal("camilla", actual.Bio.FirstName);
            Assert.Equal(2, actual.Version);
        }

        [Fact]
        public void CanSerializeDeserializeDifferentEventVersion()
        {
            // v1 configuration
            var v1_serializer = new YAMLSerializer(_v1_mapper);
            var v1_snapshots = new MemorySnapshotStore();
            var v1 = new {
                Serializer = new YAMLSerializer(_v1_mapper)
                , Store = new MemoryEventStore(_v1_mapper, v1_serializer)
                , Snapshots = v1_snapshots
                , Projections = new ProjectionManager(v1_snapshots)
            };
            v1.Projections.Register(new units.application.Employees.Projections.ActiveEmployeesProjection());
            v1.Store.AfterSave(v1.Projections.UpdateProjections);
            v1.Store.AfterDBReload(v1.Projections.UpdateProjections);
            
            // v2 configuration
            var v2_serializer = new YAMLSerializer(_v2_mapper);
            var v2_snapshots = new MemorySnapshotStore();
            var v2 = new {
                Serializer = new YAMLSerializer(_v2_mapper)
                , Store = new MemoryEventStore(_v2_mapper, v2_serializer)
                , Snapshots = v2_snapshots
                , Projections = new ProjectionManager(v2_snapshots)
            };
            v2.Projections.Register(new units.v2.application.Employees.Projections.ActiveEmployeesProjection());
            v2.Store.AfterSave(v2.Projections.UpdateProjections);
            v2.Store.AfterDBReload(v2.Projections.UpdateProjections);

            // test start
            // insert data using v1 events
            var stub1 = CreateStubV1(v1.Store, new Bio("juan", "santos", "dela cruz", "1/1/2000"));
            var stub2 = CreateStubV1(v1.Store, new Bio("camilla", "", "dela torre", "2/2/2000"), active: true);
            var stub3 = CreateStubV1(v1.Store, new Bio("juan", "", "felipe", "3/3/2000"));
            
            // serialize v1 db, and we try to read the contents using a v2_mapper
            var db = v1.Store as IYAMLSerializable;
            var raw_string = db.YAML_Export();
            
            Console.WriteLine("--------- v1 ----------");
            Console.WriteLine(raw_string);
            
            // read using v2_mapper
            v2.Store.YAML_Load(raw_string);

            // insert data using v2 events
            var stub4 = CreateStubV2(v2.Store, new Bio("antonio", "", "banderas", "4/4/2000"));
            var stub6 = CreateStubV2(v2.Store, new Bio("emilio", "", "joaquin", "5/5/2000"), active: true);

            // serialize v2 db, and we try to read the contents using a v1 mapper
            var db2 = v2.Store as IYAMLSerializable;
            var raw_string2 = db2.YAML_Export();

            Console.WriteLine("--------- v2 ----------");
            Console.WriteLine(raw_string2);

            // reload both store
            v1.Store.YAML_Load(raw_string2);
            v2.Store.YAML_Load(raw_string2);

            // fetch sample v1 events
            var v1_events = v2.Store.Get<units.domain.Employees.Employee>(stub3);

            // fetch sample v2 events
            var v2_events = v2.Store.Get<units.v2.domain.Employees.Employee>(stub6[1]);     // [owner, id]

            // test load aggregate
            var v1_actual = new units.domain.Employees.Employee();
            var v2_actual = new units.v2.domain.Employees.Employee();

            // load different events to different aggregate version
            v1_actual.Load(v2_events);
            v2_actual.Load(v1_events);

            Assert.Equal("emilio", v1_actual.Bio.FirstName);
            Assert.Equal("juan", v2_actual.Bio.FirstName);
        }
    }
}
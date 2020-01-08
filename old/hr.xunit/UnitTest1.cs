using System;
using hr.core.domain.Employees;
using hr.core.domain.commons;
using hr.core.infrastracture;
using hr.infrastracture.context;
using hr.infrastracture.serializer;
using Xunit;

namespace hr.xunit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            var serializer = new YAMLSerializer();
            var rmq = new RMQBroker(mapper, serializer, hostname: "192.168.99.100");
            var test = new EmployeesIntegrationService();

            var b = Bio.Create("Juan", "", "Dela Cruz", "", Gender.MALE, Date.TryParse("1/1/2001"));
            var e = Employee.Create(b);

            // EventBroker.getInstance().Emit(new NotCreated(e));
        }
    }
}

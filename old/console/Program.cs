using System;
using hr.core.application.Employees;
using hr.core.domain.Employees;
using hr.core.domain.Employees.events.errors;
using hr.core.domain.commons;
using hr.core.helper;
using hr.core.infrastracture;
using hr.infrastracture.context;
using hr.infrastracture.serializer;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapper_config = MappingProfile.InitializeAutoMapper();
            var serializer = new YAMLSerializer();
            var mapper = mapper_config.CreateMapper();

            var rmq = new RMQBroker(mapper, serializer, hostname: "192.168.99.100");
            
            var bio = Bio.Create("Juan", "", "Dela Cruz", "", Gender.MALE, Date.TryParse("1/1/2001"));
            var emp = Employee.Create(bio);
            
            // var d = mapper.Map(emp, typeof(Employee), typeof(EmployeeDTO));
            // var x = d as EmployeeDTO;
            // Console.WriteLine(x.Status);
            // var test = new EmployeesIntegrationService();

            // var e = Employee.Create(b);

            // EventBroker.getInstance().Emit(new NotCreated(e));
        }
    }
}

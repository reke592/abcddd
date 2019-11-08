using System.Collections.Generic;
using hr.domain.models.Companies;
using hr.domain.models.Employees;
using hr.domain.shared;
using hr.helper.database;
using hr.helper.domain;
using Xunit;

namespace hr.test {
    public class DepartmentDomainServiceTest : IDepartmentDomainServices
    {
        private static readonly IRepository<Employee> employees = new DummyRepositoryBase<Employee>();
        private static readonly IRepository<Department> departments = new DummyRepositoryBase<Department>();

        public static Employee employee1 = Employee.Create(Person.Create("Juan", "Santos", "Dela Cruz", "Jr", EnumSex.Male, Date.Create(1992, 3, 4)));
        public static Employee employee2 = Employee.Create(Person.Create("Ann", "Bernabe", "Santos", null, EnumSex.Female, Date.Create(1994, 3, 2)));
        public static Department department1 = Department.Create("Faculty", 20);
        public static Department department2 = Department.Create("Finance", 5);

        public static IEnumerable<object[]> Data = new List<object[]> {
            new object[] { employee1, department1 }
            , new object[] { employee2, department1 }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void AssignEmployeeToDepartment(Employee employee, Department department)
        {
            var broker = EventBroker.getInstance;
            broker.Command(department.addEmployee(employee));   // will emit event: EmployeeAssignedToDepartment
            
            Assert.Equal(employee.getDepartment(), department); // employee state updated via CQRS pattern
            Assert.Contains(employee, department.Employees);

            departments.Update(department);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void RemoveEmployeeFromDepartment(Employee employee, Department department)
        {
            var broker = EventBroker.getInstance;
            broker.Command(department.removeEmployee(employee));

            Assert.Null(employee.getDepartment());
            Assert.DoesNotContain(employee, department.Employees);
           
            departments.Update(department);
        }
    }
}
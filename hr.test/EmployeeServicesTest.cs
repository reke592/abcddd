using System;
using hr.domain.models.Companies;
using hr.domain.models.Employees;
using hr.domain.shared;
using hr.helper.errors;
using hr.infrastracture.database.nhibernate;
using Xunit;

namespace hr.test
{
    public class UnitTest1
    {
        [Fact]
        public void IncorrectPersonDetails_throwsErrorBag()
        {
            Action actual = () => Person.Create("erric john1", "c@stillo2", "rapsing3", "", EnumSex.Male, new DateTime(1992, 5, 24));
            Assert.Throws<ErrorBag>(actual);            
        }

        [Fact]
        public void addEmployeeToDepartment() {
            // TODO: Create IRepository base implementation
            // need to use IOC to supply repository implementations
            var employeeRepository = new EmployeeRepository();
            var departmentRepository = new DepartmentRepository();
            var service = new DepartmentServices(employeeRepository, departmentRepository);
            long emp_id = 0L;
            long dept_id = 0L;

            using(var uow = new NHUnitOfWork()) {
                var departmentstub = Department.Create("Faculty", 20);
                var personstub = Person.Create("Juan", "Santos", "Dela Cruz", null, EnumSex.Male, new DateTime(1992, 5, 24));
                var employeestub = Employee.Create(personstub);
                
                employeeRepository.Save(employeestub);
                departmentRepository.Save(departmentstub);
                uow.Commit();

                dept_id = departmentstub.Id;
                emp_id = employeestub.Id;
            }
            
            using(var uow = new NHUnitOfWork()) {
                var departmentstub = uow.Session.Get<Department>(dept_id);
                var employeestub = uow.Session.Get<Employee>(emp_id);

                service.AddEmployeeToDepartment(departmentstub, employeestub);
                uow.Commit();

                var actual = employeestub.getDepartment()?.Id;
                Assert.Equal(departmentstub.Id, actual);
            }
        }
    }
}

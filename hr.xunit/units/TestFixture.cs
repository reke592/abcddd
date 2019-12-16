using System;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using hr.com.infrastracture.database.nhibernate;
using hr.com.infrastracture.database.nhibernate.repositories;

namespace hr.xunit.units {
    public class TestFixture : IDisposable
    {

        public TestFixture() {
            ISalaryRepository salary_repository = new SalaryRepository();
            IEmployeeRepository employees_repository = new EmployeeRepository();
            using(var uow = new NHUnitOfWork()) {
                var e = Employee.Create(Person.Create("juan", "santos", "puruntong", "", Gender.MALE, Date.Now), Date.Now);
                var s = Salary.Create(e, MonetaryValue.of("php", 10000));
                var d = Deduction.Create(s, 3, MonetaryValue.of("php", 5000));
                var da = Deduction.CreateAmortized(s, 12, MonetaryValue.of("php", 200), mode: DeductionMode.CONTINIOUS);
                var db = Deduction.CreateAmortized(s, 12, MonetaryValue.of("php", 300), mode: DeductionMode.CONTINIOUS);
                var dc = Deduction.CreateAmortized(s, 12, MonetaryValue.of("php", 400), mode: DeductionMode.CONTINIOUS);

                var e2 = Employee.Create(Person.Create("juan", "cruz", "dela cruz", "", Gender.MALE, Date.Now), Date.Now);
                var s2 = Salary.Create(e2, MonetaryValue.of("php", 15000));
                var d2 = Deduction.Create(s2, 3, MonetaryValue.of("php", 3000));
                var d2a = Deduction.CreateAmortized(s2, 12, MonetaryValue.of("php", 200), mode: DeductionMode.CONTINIOUS);
                var d2b = Deduction.CreateAmortized(s2, 12, MonetaryValue.of("php", 300), mode: DeductionMode.CONTINIOUS);
                var d2c = Deduction.CreateAmortized(s2, 12, MonetaryValue.of("php", 400), mode: DeductionMode.CONTINIOUS);

                var e3 = Employee.Create(Person.Create("ann", "santos", "cruz", "", Gender.FEMALE, Date.Now), Date.Now);
                var s3 = Salary.Create(e3, MonetaryValue.of("php", 13000));
                var d3 = Deduction.Create(s3, 2, MonetaryValue.of("php", 2000));
                var d3a = Deduction.CreateAmortized(s3, 12, MonetaryValue.of("php", 200), mode: DeductionMode.CONTINIOUS);
                var d3b = Deduction.CreateAmortized(s3, 12, MonetaryValue.of("php", 300), mode: DeductionMode.CONTINIOUS);
                var d3c = Deduction.CreateAmortized(s3, 12, MonetaryValue.of("php", 400), mode: DeductionMode.CONTINIOUS);

                var e4 = Employee.Create(Person.Create("audrey", "yin", "yang", "", Gender.FEMALE, Date.Now), Date.Now);
                var s4 = Salary.Create(e4, MonetaryValue.of("php", 14000));
                var d4 = Deduction.Create(s4, 2, MonetaryValue.of("php", 5000));
                var d4a = Deduction.CreateAmortized(s4, 12, MonetaryValue.of("php", 200), mode: DeductionMode.CONTINIOUS);
                var d4b = Deduction.CreateAmortized(s4, 12, MonetaryValue.of("php", 300), mode: DeductionMode.CONTINIOUS);
                var d4c = Deduction.CreateAmortized(s4, 12, MonetaryValue.of("php", 400), mode: DeductionMode.CONTINIOUS);

                employees_repository.Save(e);
                employees_repository.Save(e2);
                employees_repository.Save(e3);
                employees_repository.Save(e4);
                salary_repository.Save(s);
                salary_repository.Save(s2);
                salary_repository.Save(s3);
                salary_repository.Save(s4);
                uow.Commit();
            }
        }

        public void Dispose()
        { }
    }
}
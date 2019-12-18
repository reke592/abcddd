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
        public static int EmployeeTestSamples = 1000;
        public TestFixture() {
            ISalaryRepository salary_repository = new SalaryRepository();
            IEmployeeRepository employees_repository = new EmployeeRepository();
            using(var uow = NHUnitOfWork.Statefull) {
                var accA = DeductionAccount.Create("A");
                var accB = DeductionAccount.Create("B");
                var accC = DeductionAccount.Create("C");
                var accD = DeductionAccount.Create("D");

                uow.Session.Save(accA);
                uow.Session.Save(accB);
                uow.Session.Save(accC);
                uow.Session.Save(accD);
                
                for(int i = 0; i < EmployeeTestSamples; i++) {
                    var e = Employee.Create(Person.Create($"fname{i}", $"mname{i}", $"sname{i}", "", Gender.MALE, Date.Now), Date.Now);
                    var s = Salary.Create(e, MonetaryValue.of("php", 10000 + i));
                    var d = Deduction.Create(s, accA, 3, MonetaryValue.of("php", 5000 + i));
                    var da = Deduction.CreateAmortized(s, accB, 12, MonetaryValue.of("php", 2000 + i), mode: DeductionMode.CONTINIOUS);
                    var db = Deduction.CreateAmortized(s, accC, 12, MonetaryValue.of("php", 300 + i), mode: DeductionMode.CONTINIOUS);
                    var dc = Deduction.CreateAmortized(s, accD, 12, MonetaryValue.of("php", 400 + i), mode: DeductionMode.CONTINIOUS);
                    uow.Session.Save(s);
                    uow.Session.Save(e);
                }
                
                uow.Commit();
            }
        }

        public void Dispose()
        { }
    }
}
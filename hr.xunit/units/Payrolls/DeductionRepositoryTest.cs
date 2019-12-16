using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using hr.com.helper.domain;
using hr.com.infrastracture.database.nhibernate;
using hr.com.infrastracture.database.nhibernate.repositories;
using Xunit;

namespace hr.xunit.units.Payrolls {
    public class DeductionRepositoryTest {
        private static readonly IEmployeeRepository _employees = new EmployeeRepository();
        private static readonly ISalaryRepository _salaries = new SalaryRepository();

        [Fact]
        public void can_get_active_deductions() {
            using(var uow = new NHUnitOfWork()) {
                var employee = Employee.Create(Person.Create("a", "a", "a", "", Gender.MALE, Date.Now), Date.Now);
                var salary = Salary.Create(employee, MonetaryValue.of("PHP", 15000));
                Deduction.Create(salary, 3, MonetaryValue.of("php", 1000));
                Deduction.Create(salary, 3, MonetaryValue.of("php", 2000));
                Deduction.Create(salary, 3, MonetaryValue.of("php", 3000));
                // Deduction.Create(salary, 3, MonetaryValue.of("php", 0));
                EventBroker.getInstance().Command(new CommandAssociateSalaryToEmployee(salary, employee));
                
                _employees.Save(employee);
                _salaries.Save(salary);
                uow.Commit();
            }

            using(var uow = new NHUnitOfWork()) {
                var ees = _employees.FetchAllActive();
                var ds = _salaries.FetchEmployeeActiveDeduction(ees[0]);
                Assert.Equal(3, ds.Count);
            }
        }
    }
}
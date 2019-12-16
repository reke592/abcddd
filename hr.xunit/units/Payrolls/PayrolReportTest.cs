using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using hr.com.helper.domain;
using hr.com.infrastracture.database.nhibernate;
using hr.com.infrastracture.database.nhibernate.repositories;
using Xunit;

namespace hr.xunit.units.Payrolls {
    public class PayrollReportTest : IClassFixture<TestFixture> {
        private static readonly IEmployeeRepository _employees = new EmployeeRepository();
        private static readonly ISalaryRepository _salaries = new SalaryRepository();
        private static readonly IPayrollRepository _payrolls = new PayrollRepository();
        
        [Fact]
        public void can_create_payroll_of_active_employees() {
            using(var uow = new NHUnitOfWork()) {
                var ees = _employees.FetchAllActive();
                var actual = ees.Count;
                var pr = PayrollReport.Create(ees, Date.Now);
                EventBroker.getInstance().Command(new CommandIncludeSalaryDeductionInReport(pr));
                Assert.True(ees.Count > 0);
                Assert.Equal(pr.Records.Count, actual);
                _payrolls.Save(pr);
                uow.Commit();
            }
        }
    }
}
using Xunit;
using hr.com.application.Payrolls;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.helper.database;
using hr.com.helper.domain;
using hr.com.infrastracture.database.nhibernate;
using hr.com.infrastracture.database.nhibernate.repositories;
using hr.com.domain.enums;

namespace hr.xunit.units.Payrolls {
    public class PayrollAppServiceTest : IClassFixture<TestFixture> {
        private static readonly ICQRSBroker<Event, Command, Query> _broker = EventBroker.getInstance();
        private static readonly IUnitOfWorkProvider<IUnitOfWork> _uow = new NHibernateHelper();
        private static readonly IPayrollDomainService _payroll_domain = new PayrollDomainService(_broker);
        private static readonly IEmployeeRepository _employee_repo = new EmployeeRepository();
        private static readonly IPayrollRepository _payroll_repo = new PayrollRepository();
        private static readonly ISalaryRepository _salary_repo = new SalaryRepository();
        private static readonly IPayrollAppService _service = new PayrollAppService(_broker, _uow, _payroll_domain, _employee_repo, _payroll_repo, _salary_repo);

        [Fact]
        public void TestGenerateReport() {
            SqlInterceptor.send_data = true;
            var dto = _service.GeneratePayroll(1, 2019, Unit.WHOLE);
            dto = _service.GeneratePayroll(1, 2019, Unit.QUARTER);
            Assert.Equal(TestFixture.EmployeeTestSamples, dto.records.Count);
        }
    }
}
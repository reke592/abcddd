using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.helper.database;
using hr.com.helper.domain;

namespace hr.com.application.Payrolls {
    public class PayrollAppService : IPayrollAppService
    {
        private readonly ICQRSBroker<Event, Command, Query> _broker;
        private readonly IUnitOfWorkProvider<IUnitOfWork> _uow;
        private readonly IPayrollDomainService _payroll_domain;
        private readonly IEmployeeRepository _employee_repo;
        private readonly IPayrollRepository _payroll_repo;
        private readonly ISalaryRepository _salary_repo;

        public PayrollAppService
        (
            ICQRSBroker<Event, Command, Query> broker
            , IUnitOfWorkProvider<IUnitOfWork> uow
            , IPayrollDomainService payroll_domain_service
            , IEmployeeRepository employee_repo
            , IPayrollRepository payroll_repo
            , ISalaryRepository salary_repo
        ) {
            _broker = broker;
            _uow = uow;
            _payroll_domain = payroll_domain_service;
            _employee_repo = employee_repo;
            _payroll_repo = payroll_repo;
            _salary_repo = salary_repo;
        }

        public PayrollReportDTO GeneratePayroll(int month, int year, double month_unit, bool include_deductions = true)
        {
            using(var transaction = _uow.CreateTransaction()) {
                var ees = _employee_repo.FetchAllActive();
                var pr = _payroll_domain.GeneratePayrollReport(ees, month, year, include_deductions, month_unit);              
                
                _payroll_repo.Save(pr);
                transaction.Commit();
                return new PayrollReportDTO(pr);
            }
        }
    }
}
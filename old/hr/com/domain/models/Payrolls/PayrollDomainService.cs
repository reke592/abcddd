using System.Collections.Generic;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class PayrollDomainService : IPayrollDomainService
    {
        private readonly ICQRSBroker<Event, Command, Query> _broker;

        public PayrollDomainService(ICQRSBroker<Event, Command, Query> broker) {
            this._broker = broker;
        }

        public PayrollReport GeneratePayrollReport(IList<Employee> employees, int month, int year, bool include_deductions, double month_unit)
        {
            var pr = PayrollReport.Create(employees, Date.Create(year, month, 1), month_unit);
            if(include_deductions) {
                _broker.Command(new CommandIncludeSalaryDeductionInReport(pr, month_unit));
            }
            return pr;
        }

        public Deduction AddSalaryDeduction(Employee employee, Deduction deduction)
        {
            throw new System.NotImplementedException();
        }

        public Salary SetEmployeeSalary(Employee employee, Salary salary)
        {
            throw new System.NotImplementedException();
        }
    }
}
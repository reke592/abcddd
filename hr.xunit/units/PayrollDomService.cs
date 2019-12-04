using System.Collections.Generic;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;

namespace hr.xunit.units {
    public class PayrollDomService : IPayrollDomainService
    {
        public void AddSalaryDeduction(Employee employee, Deduction deduction)
        {
            throw new System.NotImplementedException();
        }

        public PayrollRecord GeneratePayroll(LongMonth month, IList<Employee> employees)
        {
            throw new System.NotImplementedException();
        }

        public PayrollReport GeneratePayroll(int month, int year, IList<Employee> employees)
        {
            throw new System.NotImplementedException();
        }

        public void SetEmployeeSalary(Employee employee, Salary salary)
        {
            throw new System.NotImplementedException();
        }
    }
}
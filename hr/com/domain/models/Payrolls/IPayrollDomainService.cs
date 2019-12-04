using System.Collections.Generic;
using hr.com.domain.models.Employees;
using hr.com.domain.enums;

namespace hr.com.domain.models.Payrolls {
    public interface IPayrollDomainService {
        PayrollReport GeneratePayroll(int month, int year, IList<Employee> employees);
        void SetEmployeeSalary(Employee employee, Salary salary);
        void AddSalaryDeduction(Employee employee, Deduction deduction);
    }
}
using System.Collections.Generic;
using hr.com.domain.models.Employees;
using hr.com.domain.enums;

namespace hr.com.domain.models.Payrolls {
    public interface IPayrollDomainService {
        PayrollReport GeneratePayrollReport(IList<Employee> employees, int month, int year, bool include_deductions = true, double month_unit = Unit.WHOLE);
        Salary SetEmployeeSalary(Employee employee, Salary salary);
        Deduction AddSalaryDeduction(Employee employee, Deduction deduction);
    }
}
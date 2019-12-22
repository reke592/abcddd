using System.Collections.Generic;
using hr.core.domain.Employees;
using hr.core.domain.shared;

namespace hr.core.domain.Payrolls {
    public interface ISalaryDeductionService {
        // single entry
        SalaryDeductionResult AddSalaryDeduction(Salary salary, DeductionAccount account, MonetaryValue total, decimal amortization);
        
        // bulk entry
        SalaryDeductionResult AddSalaryDeduction(IEnumerable<Salary> salaries, DeductionAccount account, MonetaryValue total, decimal amortization);
        
        // once granted, payroll deduction will start
        SalaryDeductionResult Grant(IEnumerable<Deduction> deduction);

        // deny request
        SalaryDeductionResult Deny(IEnumerable<Deduction> deduction);

        SalaryDeductionResult RequestCancel(Deduction deduction);
    }
}
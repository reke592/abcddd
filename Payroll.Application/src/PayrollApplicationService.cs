using Payroll.Application.BusinessYears;
using Payroll.Application.Deductions;
using Payroll.Application.Employees;
using Payroll.Application.PayrollPeriods;
using Payroll.Application.SalaryGrades;
using Payroll.Application.Users;

namespace Payroll.Application
{
  public class PayrollApplicationService : IPayrollApplicationServices
  {
    private readonly IUserAppService _user;
    private readonly IBusinessYearAppService _businessYear;
    private readonly IPayrollPeriodAppService _payrollPeriod;
    private readonly IEmployeeAppService _employee;
    private readonly ISalaryGradeAppService _salaryGrade;
    private readonly IDeductionAppService _deduction;

    public PayrollApplicationService(
      IUserAppService user,
      IBusinessYearAppService businessYear,
      IPayrollPeriodAppService payrollPeriod,
      IEmployeeAppService employee,
      ISalaryGradeAppService salaryGrade,
      IDeductionAppService deduction
    ) {
      _user = user;
      _businessYear = businessYear;
      _payrollPeriod = payrollPeriod;
      _employee = employee;
      _salaryGrade = salaryGrade;
      _deduction = deduction;
    }


    public IUserAppService User => _user;
    public IBusinessYearAppService BusinessYear => _businessYear;
    public IPayrollPeriodAppService PayrollPeriod => _payrollPeriod;
    public IEmployeeAppService Employee => _employee;
    public ISalaryGradeAppService SalaryGrade => _salaryGrade;
    public IDeductionAppService Deduction => _deduction;
  }
}
using Payroll.Application.BusinessYears;
using Payroll.Application.Deductions;
using Payroll.Application.Employees;
using Payroll.Application.PayrollPeriods;
using Payroll.Application.SalaryGrades;
using Payroll.Application.Users;

namespace Payroll.Application
{
  public interface IPayrollApplicationServices
  {
      IUserAppService User { get; }
      IBusinessYearAppService BusinessYear { get; }
      IPayrollPeriodAppService PayrollPeriod { get; }
      IEmployeeAppService Employee { get; }
      ISalaryGradeAppService SalaryGrade { get; }
      IDeductionAppService Deduction { get; }
  }
}
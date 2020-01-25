using Payroll.Domain.BusinessYears;
using Payroll.Domain.Deductions;
using Payroll.Domain.Employees;
using Payroll.Domain.PayrollPeriods;
using Payroll.Domain.Shared;

namespace Payroll.Application.PayrollPeriods
{
  public static class Contracts
  {
    public static class V1
    {
      public class CreatePayrollPeriod
      {
        public string AccessToken { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public LongMonth ApplicableMonth { get; set; }
      }

      public class AddPayrollConsignee
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ConsigneeAction { get; set; }
      }

      public class RemovePayrollConsignee
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ConsigneeAction { get; set; }
      }

      public class ExcludeEmployeesToPayroll
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public EmployeeId[] EmployeeIds { get; set; }
      }

      public class IncludeEmployeesToPayroll
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public EmployeeId[] EmployeeIds { get; set; }
      }

      public class AdjustPayrollDeductionPayment
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public EmployeeId EmployeeId { get; set; }
        public DeductionId DeductionId { get; set; }
        public decimal AdjustedAmount { get; set; }
      }

      public class DispenseEmployeeSalary
      {
        public string AccessToken { get; set; }
        public PayrollPeriodId PayrollPeriodId { get; set; }
        public EmployeeId EmployeeId { get; set; }
      }
    }
  }
}
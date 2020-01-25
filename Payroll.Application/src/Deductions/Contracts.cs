using Payroll.Domain.BusinessYears;
using Payroll.Domain.Deductions;
using Payroll.Domain.Employees;

namespace Payroll.Application.Deductions
{
  public static class Contracts
  {
    public static class V1
    {
      // TODO: create deduction account
      public class CreateDeduction
      {
        public string AccessToken { get; set; }
        public EmployeeId EmplyoeeId { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public decimal Amount { get; set; }
        public int Amortization { get; set; } = 1;
        public DeductionSchedule Schedule { get; set; }
      }

      public class CreateDeductionPayment
      {
        public string AccessToken { get; set; }
        public DeductionId DeductionId { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public decimal Payment { get; set; }
      }

      public class StopDeduction // --> complete deduction
      {
        public string AccessToken { get; set; }
        public DeductionId DeductionId { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
      }
    }
  }
}
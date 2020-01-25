using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.Domain.Users;

namespace Payroll.Domain.PayrollPeriods
{
  public static class Events
  {
    public static class V1
    {
      public class PayrollPeriodCreated
      {
        public PayrollPeriodId Id { get; set; }
        public BusinessYearId BusinessYear { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class PayrollPeriodApplicableMonthSettled
      {
        public PayrollPeriodId Id { get; set; }
        public LongMonth NewApplicableMonth { get; set; }
        public UserId SettledBy { get; set; }
        public DateTimeOffset SettledAt { get; set; }
      }

      public class PayrollPeriodConsigneeIncluded
      {
        public PayrollPeriodId Id { get; set; }
        // public ConsigneePerson Consignee { get; set; }
        // public string ConsigneeAction { get; set; }
        public PayrollConsignee Consignee { get; set; }
        public UserId IncludedBy { get; set; }
        public DateTimeOffset IncludedAt { get; set; }
      }

      public class PayrollPeriodConsigneeRemoved
      {
        public PayrollPeriodId Id { get; set; }
        // public ConsigneePerson Consignee { get; set; }
        public PayrollConsignee Consignee { get; set; }
        public UserId RemovedBy { get; set; }
        public DateTimeOffset RemovedAt { get; set; }
      }

      public class PayrollPeriodEmployeeIncluded
      {
        public PayrollPeriodId Id { get; set; }
        public EmployeeId Employee { get; set; }
        public UserId IncludedBy { get; set; }
        public DateTimeOffset IncludedAt { get; set; }
      }

      public class PayrollPeriodEmployeeExcluded
      {
        public PayrollPeriodId Id { get; set; }
        public EmployeeId Employee { get; set; }
        public UserId ExcludedBy { get; set; }
        public DateTimeOffset ExcludedAt { get; set; }
      }

      public class PayrollPeriodEmployeeSalaryReceived
      {
        public PayrollPeriodId Id { get; set; }
        public EmployeeId Employee { get; set; }
        public UserId GivenBy { get; set; }
        public DateTimeOffset GivenAt { get; set; }
      }

      public class PayrollPeriodDeductionPaymentAdjusted
      {
        public PayrollPeriodId Id { get; set; }
        public AdjustedDeductionPayment Adjusment { get; set; }
        public UserId AdjustedBy { get; set; }
        public DateTimeOffset AdjustedAt { get; set; }
      }

      public class PayrollPeriodUpdateAttemptFailed
      {
        public PayrollPeriodId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }

    }
  }
}
using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.Deductions
{
  public static class Events
  {
    public static class V1
    {

      public class DeductionCreated
      {
        public DeductionId Id { get; set; }
        public EmployeeId Employee { get; set; }
        public BusinessYearId BusinessYear { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      // public class DeductionAmountSettled
      // {
      //   public DeductionId Id { get; set; }
      //   public decimal NewAmount { get; set; }
      //   public UserId SettledBy { get; set; }
      //   public DateTimeOffset SettledAt { get; set; }
      // }

      public class DeductionScheduleSettled
      {
        public DeductionId Id { get; set; }
        public int NewAmortization { get; set; }
        public decimal AmortizedAmount { get; set; }
        public DeductionSchedule NewSchedule { get; set; }
        public UserId SettledBy { get; set; }
        public DateTimeOffset SettledAt { get; set; }
      }

      public class DeductionPaymentCreated
      {
        public DeductionId Id { get; set; }
        public decimal PaidAmount { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class DeductionPaymentCompleted
      {
        public DeductionId Id { get; set; }
        public BusinessYearId BusinessYear { get; set; }
        public decimal PaymentTotal { get; set; }
        public UserId SettledBy { get; set; }
        public DateTimeOffset CompletedAt { get; set; }
      }

      public class DeductionUpdateAttemptFailed
      {
        public DeductionId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }
    }
  }
}
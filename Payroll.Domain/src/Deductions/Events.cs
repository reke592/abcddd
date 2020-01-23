using System;
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
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class DeductionBalanceUpdated
      {
        public DeductionId Id { get; set; }
        public decimal NewBalance { get; set; }
        public UserId UpdatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
      }

      public class DeductionPaymentCreated
      {
        public DeductionId Id { get; set; }
        public decimal PaidAmount { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
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
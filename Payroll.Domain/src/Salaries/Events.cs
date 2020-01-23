using System;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;

namespace Payroll.Domain.Salaries
{
  public static class Events
  {
    public static class V1
    {
      public class SalaryCreated
      {
        public SalaryId Id { get; set; }
        public EmployeeId Employee { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class SalaryGrossUpdated
      {
        public SalaryId Id { get; set; }
        public decimal NewGrossValue { get; set; }
        public UserId UpdatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
      }

      public class SalaryUpdateFailed
      {
        public SalaryId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }
    }
  }
}
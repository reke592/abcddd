using System;
using Payroll.Domain.SalaryGrades;
using Payroll.Domain.Users;

namespace Payroll.Domain.Employees
{
  public static class Events
  {
    public class V1
    {
      public class EmployeeCreated
      {
        public EmployeeId Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserId CreatedBy { get; set; }
      }

      public class EmployeeBioDataUpdated
      {
        public EmployeeId Id { get; set; }
        public BioData BioData { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public UserId UpdatedBy { get; set; }
      }

      public class EmployeeStatusChanged
      {
        public EmployeeId Id { get; set; }
        public EmployeeStatus NewStatus { get; set; }
        public UserId ChangedBy { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
      }

      public class EmployeeSalaryGradeUpdated
      {
        public EmployeeId Id { get; set; }
        public SalaryGradeId SalaryGrade { get; set; }
        public UserId UpdatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
      }

      public class EmployeeUpdateAttemptFailed
      {
        public EmployeeId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }
    }
  }
}
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
        public BioData BioData { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserId CreatedBy { get; set; }
      }


      public class EmployeeEmployed
      {
        public EmployeeId Id { get; set; }
        public BioData BioData { get; set; }
        public EmployeeStatus NewStatus { get; set; }
        public DateTimeOffset EmployedAt { get; set; }
        public UserId EmployedBy { get; set; }
      }

      public class EmployeeSeparated
      {
        public EmployeeId Id { get; set; }
        public BioData BioData { get; set; }
        public EmployeeStatus NewStatus { get; set; }
        public UserId SeparatedBy { get; set; }
        public DateTimeOffset SeparatedAt { get; set; }
      }

      public class EmployeeBioDataUpdated
      {
        public EmployeeId Id { get; set; }
        public BioData BioData { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public UserId UpdatedBy { get; set; }
      }

      // public class EmployeeStatusEmployed
      // {
      //   public EmployeeId Id { get; set; }
      //   public BioData BioData { get; set; }
      //   public UserId SettledBy { get; set; }
      //   public DateTimeOffset SettledAt { get; set; }
      // }

      public class EmployeeLeaveGranted
      {
        public EmployeeId Id { get; set; }
        // public BioData BioData { get; set; }
        public EmployeeLeaveRequest LeaveRequest { get; set; }
        public UserId GrantedBy { get; set; }
        public DateTimeOffset GrantedAt { get; set; }
      }

      public class EmployeeLeaveEnded
      {
        public EmployeeId Id { get; set; }
        // public BioData BioData { get; set; }
        public EmployeeLeaveRequest EndedLeaveRequest { get; set; }
        public UserId EndedBy { get; set; }
        public DateTimeOffset EndedAt { get; set; }
      }

      public class EmployeeLeaveRevoked
      {
        public EmployeeId Id { get; set; }
        public EmployeeLeaveRequest RevokedLeaveRequest { get; set; }
        public UserId RevokedBy { get; set; }
        public DateTimeOffset RevokedAt { get; set; }
      }

      public class EmployeeSalaryGradeUpdated
      {
        public EmployeeId Id { get; set; }
        public SalaryGradeId SalaryGradeId { get; set; }
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
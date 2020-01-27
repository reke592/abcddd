using Payroll.Domain.Employees;
using Payroll.Domain.SalaryGrades;
using Payroll.Domain.Shared;

namespace Payroll.Application.Employees
{
  public static class Contracts
  {
    public static class V1
    {
      public class CreateEmployee
      {
        public string AccessToken { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
      }

      public class UpdateBioData
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
      }

      public class UpdateSalaryGrade
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
        public SalaryGradeId SalaryGradeId { get; set; }
      }

      // public class ChangeStatus
      // {
      //   public string AccessToken { get; set; }
      //   public EmployeeId EmplyoeeId { get; set; }
      //   public EmployeeStatus NewStatus { get; set; }
      // }

      public class EmployEmployee
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
      }

      public class SeparateEmployee
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
      }

      public class GrantLeave
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
        public Date Start { get; set; }
        public Date Return { get; set; }
      }

      public class RevokeLeave
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
      }

      public class EndLeave
      {
        public string AccessToken { get; set; }
        public EmployeeId EmployeeId { get; set; }
      }
    }
  }
}
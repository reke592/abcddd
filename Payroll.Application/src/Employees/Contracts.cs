using Payroll.Domain.Employees;
using Payroll.Domain.SalaryGrades;

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

      public class ChangeStatus
      {
        public string AccessToken { get; set; }
        public EmployeeId EmplyoeeId { get; set; }
        public EmployeeStatus NewStatus { get; set; }
      }
    }
  }
}
using Payroll.Domain.BusinessYears;
using Payroll.Domain.SalariesGrades;

namespace Payroll.Application.SalaryGrades
{
  public static class Contracts
  {
    public static class V1
    {
      public class CreateSalaryGrade
      {
        public string AccessToken { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public decimal GrossValue { get; set; }
      }

      public class UpdateSalaryGrade
      {
        public string AccessToken { get; set; }
        public SalaryGradeId SalaryGradeId { get; set; }
        public decimal NewGrossValue { get; set; }
      }
    }
  }
}
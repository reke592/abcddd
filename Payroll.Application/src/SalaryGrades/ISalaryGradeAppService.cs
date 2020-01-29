namespace Payroll.Application.SalaryGrades
{
  public interface ISalaryGradeAppService
  : IHandleCommand<Contracts.V1.CreateSalaryGrade>
  , IHandleCommand<Contracts.V1.UpdateSalaryGrade>
  { }
}
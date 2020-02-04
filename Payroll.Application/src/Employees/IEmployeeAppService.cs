using System;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees
{
  public interface IEmployeeAppService
  : IHandleCommand<Contracts.V1.CreateEmployee, EmployeeId>
  , IHandleCommand<Contracts.V1.EmployEmployee>
  , IHandleCommand<Contracts.V1.EndLeave>
  , IHandleCommand<Contracts.V1.GrantLeave>
  , IHandleCommand<Contracts.V1.RevokeLeave>
  , IHandleCommand<Contracts.V1.SeparateEmployee>
  , IHandleCommand<Contracts.V1.UpdateBioData>
  , IHandleCommand<Contracts.V1.UpdateSalaryGrade>
  { }
}
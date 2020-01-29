namespace Payroll.Application.PayrollPeriods
{
  public interface IPayrollPeriodAppService
  : IHandleCommand<Contracts.V1.AddPayrollConsignee>
  , IHandleCommand<Contracts.V1.AdjustPayrollDeductionPayment>
  , IHandleCommand<Contracts.V1.CreatePayrollPeriod>
  , IHandleCommand<Contracts.V1.DispenseEmployeeSalary>
  , IHandleCommand<Contracts.V1.ExcludeEmployeesToPayroll>
  , IHandleCommand<Contracts.V1.IncludeEmployeesToPayroll>
  , IHandleCommand<Contracts.V1.RemovePayrollConsignee>
  { }
}
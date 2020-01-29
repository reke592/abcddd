namespace Payroll.Application.Deductions
{
  public interface IDeductionAppService
  : IHandleCommand<Contracts.V1.CreateDeduction>
  , IHandleCommand<Contracts.V1.CreateDeductionPayment>
  , IHandleCommand<Contracts.V1.StopDeduction>
  { }
}
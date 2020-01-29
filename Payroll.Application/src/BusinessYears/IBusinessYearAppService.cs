namespace Payroll.Application.BusinessYears
{
  public interface IBusinessYearAppService
  : IHandleCommand<Contracts.V1.CreateBusinessYear>
  , IHandleCommand<Contracts.V1.CreateConsignee>
  , IHandleCommand<Contracts.V1.EndBusinessYear>
  , IHandleCommand<Contracts.V1.StartBusinessYear>
  , IHandleCommand<Contracts.V1.UpdateConsignee>
  { }
}

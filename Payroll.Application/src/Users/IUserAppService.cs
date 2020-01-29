namespace Payroll.Application.Users
{
  public interface IUserAppService
  : IHandleCommand<Contracts.V1.ChangePassword>
  , IHandleCommand<Contracts.V1.CreateUser>
  , IHandleCommand<Contracts.V1.PasswordLogin, string>
  // , IHandleCommand<Contracts.V1.TokenLogin>
  { }
}
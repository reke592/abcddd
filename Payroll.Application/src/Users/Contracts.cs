using Payroll.Domain.Users;

namespace Payroll.Application.Users
{
  public static class Contracts
  {
    public static class V1
    {
      public class CreateUser
      {
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
      }

      public class ChangePassword
      {
        public string AccessToken { get; set; }
        public UserId UserId { get; set; }
        public string NewPassword { get; set; }
      }

      public class PasswordLogin
      {
        public string Username { get; set; }
        public string Password { get; set; }
      }

      public class TokenLogin
      {
        public string AccessToken { get; set; }
      }
    }
  }
}
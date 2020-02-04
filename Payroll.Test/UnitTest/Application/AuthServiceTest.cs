using System;
using Payroll.Application.Exceptions;
using Xunit;
using static Payroll.Application.Users.Contracts.V1;

namespace Payroll.Test.UnitTest.Application
{
  public class AuthServiceTest : TestBase
  {
    [Fact]
    public void CanLogin()
    {
      _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "p4ssw0d"
      }, result => Assert.NotNull(result));
    }

    [Fact]
    public void InvalidLoginThrowsException()
    {
      Action invalid_password = () => _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "1234"
      }, result => Assert.Null(result));

      Action non_existing_user = () => _auth.Handle(new PasswordLogin {
        Username = "nobadi",
        Password = "1234"
      }, result => Assert.Null(result));

      Assert.Throws<UserLoginException>(invalid_password);
      Assert.Throws<UserLoginException>(non_existing_user);
    }
  }
}
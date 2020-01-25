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
      var actual = _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "p4ssw0d"
      });
    }

    [Fact]
    public void InvalidLoginThrowsException()
    {
      Action invalid_password = () => _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "1234"
      });

      Action non_existing_user = () => _auth.Handle(new PasswordLogin {
        Username = "nobadi",
        Password = "1234"
      });

      Assert.Throws<UserLoginException>(invalid_password);
      Assert.Throws<UserLoginException>(non_existing_user);
    }
  }
}
using System;

namespace Payroll.Application.Exceptions
{
  public class UserLoginException : Exception
  {
    public UserLoginException(string message = "Invalid username or password") : base(message) { }
  }
}
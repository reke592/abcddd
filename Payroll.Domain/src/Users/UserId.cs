using System;

namespace Payroll.Domain.Users
{
  public class UserId
  {
    private Guid _value;

    public UserId(Guid value)
    {
      _value = value;
    }

    public static implicit operator UserId(Guid value) => new UserId(value);
    public static implicit operator Guid(UserId self) => self._value;
  }
}
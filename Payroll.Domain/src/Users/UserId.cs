using System;

namespace Payroll.Domain.Users
{
  public class UserId : AggregateId<UserId>
  {
    public UserId() {}
    public UserId(Guid value)
    {
      Value = value;
    }

    public static implicit operator UserId(Guid value) => new UserId(value);
    public static implicit operator Guid(UserId self) => self.Value;
  }
}
using System;

namespace Payroll.Domain.Users
{
  public class UserId : DomainAggregateGuid
  {
    public UserId() {}
    public UserId(Guid value) : base(value) { }

    public static implicit operator UserId(Guid value) => new UserId(value);
    public static implicit operator UserId(string value) => new UserId(Guid.Parse(value));
  }
}
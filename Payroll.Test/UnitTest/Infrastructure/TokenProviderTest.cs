using System;
using Payroll.Application.Users.Projections;
using Payroll.Domain.Users;
using Payroll.Infrastructure;
using Xunit;
using static Payroll.Application.Users.Contracts.V1;
using static Payroll.Domain.Users.Events.V1;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class TokenProviderTest : TestBase
  {
    [Fact]
    public void CanCreateToken()
    {
      var _provider = new TokenProvider("some secret", _snapshots);
      var record = _snapshots.Get<ActiveUserRecord>(_rootId);
      var actual = _provider.CreateToken(record);
      Assert.True(_provider.IsValidToken(actual));
    }   
  }
}
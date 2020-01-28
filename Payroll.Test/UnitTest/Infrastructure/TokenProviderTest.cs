using Payroll.Application.Users.Projections;
using Payroll.Infrastructure;
using Xunit;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class TokenProviderTest : TestBase
  {
    [Fact]
    public void CanCreateToken()
    {
      var _provider = new TokenProvider("some secret", _cache);
      var record = _cache.Get<ActiveUsersProjection.ActiveUserRecord>(_rootId);
      var actual = _provider.CreateToken(record);
      Assert.True(_provider.IsValidToken(actual));
    }

    [Fact]
    public void CanReadToken()
    {
      var active = _cache.Get<ActiveUsersProjection.ActiveUserRecord>(_rootId);
      _tokenProvider.ReadToken(_accessTokenStub, user => {
        Assert.Equal("test", user.Username);
        return;
      });
    }
  }
}
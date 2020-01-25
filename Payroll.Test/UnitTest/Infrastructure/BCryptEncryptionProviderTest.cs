using Payroll.Infrastructure;
using Xunit;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class BCryptEncryptionProviderTest
  {
    [Fact]
    public void CanCreateHash()
    {
      var enc = new BCryptEncryptionProvider();
      var cipher = enc.CreateHash("p4ssw0d");
    }

    [Fact]
    public void CanVerifyPassword()
    {
      var enc = new BCryptEncryptionProvider();
      var passHash = "$2a$11$VQiq2RnPyec5V9D3bGX1CufYgOK8jtKwZwD0nHqkQAQKQ7kWKIPYK";
      Assert.True(enc.Test("p4ssw0d", passHash));
    }
  }
}
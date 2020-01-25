using Payroll.Application;
using Algo = BCrypt.Net;

namespace Payroll.Infrastructure
{
  public class BCryptEncryptionProvider : IEncryptionProvider
  {
    public string CreateHash(string plain_text) {
      return Algo.BCrypt.HashPassword(plain_text);
    }

    public bool Test(string plain_text, string hash) {
      return Algo.BCrypt.Verify(plain_text, hash);
    }
  }
}
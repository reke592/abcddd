namespace Payroll.Application
{
  public interface IEncryptionProvider
  {
    string CreateHash(string plain_text);
    bool Test(string plain_text, string hash);
  }
}
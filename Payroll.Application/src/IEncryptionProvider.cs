namespace Payroll.Application
{
  public interface IPasswordServiceProvider
  {
    string CreateHash(char[] plain_text);
    bool Test(string hash, char[] plain_text);
  }
}
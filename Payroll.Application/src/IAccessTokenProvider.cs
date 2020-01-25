using System;
using Payroll.Application.Users.Projections;
using Payroll.Domain.Users;

namespace Payroll.Application
{
  public interface IAccessTokenProvider
  {
    void ReadToken(string token, Action<ActiveUserRecord> cb);
    string CreateToken(ActiveUserRecord user);
  }
}
using System;
using Payroll.Application.Users.Projections;
using Payroll.Domain.Users;

namespace Payroll.Application
{
  public interface IAccessTokenProvider
  {
    void ReadToken(string token, Action<ActiveUsersProjection.ActiveUserRecord> cb);
    string CreateToken(ActiveUsersProjection.ActiveUserRecord user);
    bool IsValidToken(string token);
  }
}
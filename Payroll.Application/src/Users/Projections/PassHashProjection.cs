using System.Collections.Generic;
using Payroll.Domain.Users;
using Payroll.EventSourcing;

namespace Payroll.Application.Users.Projections
{
  public class UserPassHashRecord
  {
    public UserId UserId { get; internal set; }
    public string Username { get; internal set; }
    public string PassHash { get; internal set; }
    public int Version { get; internal set; } = 0;
  }

  public class PassHashProjection : IProjection
  {
    public void Handle(object e, ICacheStore snapshots) {
      UserPassHashRecord doc;
      switch(e)
      {
        case Events.V1.UserCreated x:
          doc = new UserPassHashRecord();
          doc.UserId = x.Id.ToString();
          doc.Username = x.Username;
          doc.PassHash = x.PassHash;
          snapshots.Store(x.Id, doc);
          break;
        
        case Events.V1.UserPasswordChanged x:
          snapshots.UpdateIfFound<UserPassHashRecord>(x.Id, r => {
            r.PassHash = x.NewPassHash;
            r.Version ++;
          });
          break;
      }
    }
  }
}
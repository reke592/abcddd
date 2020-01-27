using System.Collections.Generic;
using Payroll.Domain.Users;
using Payroll.EventSourcing;

namespace Payroll.Application.Users.Projections
{
  public class ActiveUsersProjection : IProjection
  {
    public class ActiveUserRecord
    {
      public UserId Id { get; internal set; }
      public string Username { get; internal set; }
      public ISet<string> Claims { get; internal set; } = new HashSet<string>();
      public int Version { get; internal set; } = 0;
    }
    
    public void Handle(object e, ISnapshotStore snapshots) {
      ActiveUserRecord doc;
      switch(e)
      {
        case Events.V1.UserCreated x:
          doc = new ActiveUserRecord();
          doc.Id = x.Id;
          doc.Username = x.Username;
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
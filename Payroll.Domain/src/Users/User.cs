using System;
using System.Collections.Generic;

namespace Payroll.Domain.Users
{
  public class User : Aggregate
  {
    private UserId _owner;
    public string UserName { get; set; }
    private string _passHash;
    private ISet<string> _claims = new HashSet<string>();

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.UserCreated x:
          Id = x.Id;
          UserName = x.Username;
          _passHash = x.PassHash;
          _owner= x.CreatedBy;
          break;

        case Events.V1.UserPasswordChanged x:
          _passHash = x.NewPassHash;
          break;
      }
    }

    public static User Create(UserId id, string username, string passHash, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new User();
      record.Apply(new Events.V1.UserCreated {
        Id = id,
        Username = username,
        PassHash = passHash,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    public void changePassword(string newPassHash, UserId changedBy, DateTimeOffset changedAt)
    {
      if(changedBy != _owner && changedBy != Id)
        _updateFailed("can't change password. not the record owner nor the user", newPassHash, changedBy, changedAt);
      else
        this.Apply(new Events.V1.UserPasswordChanged {
          Id = this.Id,
          NewPassHash = newPassHash,
          ChangedBy = changedBy,
          ChangedAt = changedAt
        });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.UserUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
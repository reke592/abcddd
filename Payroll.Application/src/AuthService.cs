using System;
using System.Linq;
using Payroll.Application.Exceptions;
using Payroll.Application.Users.Projections;
using Payroll.Domain.Users;
using Payroll.EventSourcing;
using UserCommands = Payroll.Application.Users.Contracts.V1;

namespace Payroll.Application
{
  public class AuthService
  {
    private readonly IAccessTokenProvider _tokenService;
    private readonly IEventStore _eventStore;
    private readonly ICacheStore _snapshots;
    private readonly IEncryptionProvider _enc;

    public AuthService(IEventStore eventStore, IAccessTokenProvider tokenService, ICacheStore snapshots, IEncryptionProvider enc)
    {
      _tokenService = tokenService;
      _eventStore = eventStore;
      _snapshots = snapshots;
      _enc = enc;
    }

    public void Handle(UserCommands.CreateUser cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = User.Create(Guid.NewGuid(), cmd.Username, _enc.CreateHash(cmd.Password), user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(UserCommands.ChangePassword cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<User>(cmd.UserId, out var events))
        {
          var record = new User();
          record.Apply(events);
          record.changePassword(_enc.CreateHash(cmd.NewPassword), user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    // TODO: how about, use an out var to avoid null checks?
    public string Handle(UserCommands.PasswordLogin cmd)
    {
      var user = _snapshots.All<UserPassHashRecord>().Where(x => x.Username == cmd.Username).SingleOrDefault();
      
      if(user is null)
        throw new UserLoginException();

      if(_enc.Test(cmd.Password, user.PassHash) == true)
        return _tokenService.CreateToken(_snapshots.Get<ActiveUsersProjection.ActiveUserRecord>(user.Id));
      else
        throw new UserLoginException();
    }
  }
}
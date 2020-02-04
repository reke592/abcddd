using System;
using System.Linq;
using Payroll.Application.Exceptions;
using Payroll.Application.Users;
using Payroll.Application.Users.Projections;
using Payroll.Domain.Users;
using Payroll.EventSourcing;
using static Payroll.Application.Users.Projections.ActiveUsersProjection;
using static Payroll.Domain.Users.Events.V1;
using UserCommands = Payroll.Application.Users.Contracts.V1;

namespace Payroll.Application
{
  public class AuthService : IUserAppService
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

      _eventStore.AfterDBReload((_, __) => this.InstallRoot());
    }

    internal void InstallRoot()
    {
      if(_snapshots.All<UserPassHashRecord>().Where(x => x.Username == "root").SingleOrDefault() is null)
      {
        var id = Guid.NewGuid();
        var root_event = new UserCreated {
          Id = id,
          Username = "root",
          PassHash = "$2a$11$VQiq2RnPyec5V9D3bGX1CufYgOK8jtKwZwD0nHqkQAQKQ7kWKIPYK",
          CreatedBy = id,
          CreatedAt = DateTimeOffset.Now
        };
        var stubRoot = new User();
        stubRoot.Apply(root_event);
        _eventStore.Save(stubRoot);
      }
    }

    public void Handle(UserCommands.CreateUser cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        // TODO: create a metod that accepts specifications in ravendb adapter for efficient queries
        var exist = _snapshots.All<ActiveUserRecord>().Where(x => x.Username == cmd.Username).ToList();
        if(exist.Count() > 0)
          throw new Exception("Username already taken");
        
        var record = User.Create(Guid.NewGuid(), cmd.Username, _enc.CreateHash(cmd.Password), user.UserId, DateTimeOffset.Now);
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
          record.changePassword(_enc.CreateHash(cmd.NewPassword), user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    // TODO: how about, use an out var to avoid null checks?
    public void Handle(UserCommands.PasswordLogin cmd, Action<string> cb)
    {
      var user = _snapshots.All<UserPassHashRecord>().Where(x => x.Username == cmd.Username).FirstOrDefault();
      
      if(user is null)
        throw new UserLoginException();

      if(_enc.Test(cmd.Password, user.PassHash) == true)
        cb(_tokenService.CreateToken(_snapshots.Get<ActiveUsersProjection.ActiveUserRecord>(user.UserId)));
      else
        throw new UserLoginException();
    }
  }
}
using System;
using Payroll.Domain.Users;
using Payroll.EventSourcing;
using UserCommands = Payroll.Application.Users.Contracts.V1;

namespace Payroll.Application
{
  public class AuthService
  {
    private readonly IAccessTokenProvider _tokenService;
    private readonly IEventStore _eventStore;

    public AuthService(IEventStore eventStore, IAccessTokenProvider tokenService)
    {
      _tokenService = tokenService;
      _eventStore = eventStore;
    }

    public void Handle(UserCommands.CreateUser cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = User.Create(Guid.NewGuid(), cmd.Username, cmd.Password, user.Id, DateTimeOffset.Now);
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
          record.changePassword(cmd.NewPassword, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public string Handle(UserCommands.PasswordLogin cmd)
    {
      // fetch user in db
      // test password
      // return access token
      return null;
    }
  }
}
using System;
using Payroll.Domain.BusinessYears;
using Payroll.EventSourcing;

namespace Payroll.Application.BusinessYears
{
  public class BusinessYearAppService : IBusinessYearAppService
  {
    private readonly IAccessTokenProvider _tokenProvider;
    private readonly IEventStore _eventStore;

    public BusinessYearAppService(IAccessTokenProvider tokenProvider, IEventStore eventStore)
    {
      _tokenProvider = tokenProvider;
      _eventStore = eventStore;
    }

    public void Handle(Contracts.V1.CreateBusinessYear cmd, Action<BusinessYearId> cb)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        var record = BusinessYear.Create(Guid.NewGuid(), cmd.ApplicableYear, user.UserId, DateTimeOffset.Now);
        _eventStore.Save(record);
        cb(record.Id);
      });
    }

    public void Handle(Contracts.V1.StartBusinessYear cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          record.Load(events);
          record.Start(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.EndBusinessYear cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          record.Load(events);
          record.End(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.CreateConsignee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          record.Load(events);
          record.addConsignee(ConsigneePerson.Create(cmd.Name, cmd.Position), user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.UpdateConsignee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          var old = ConsigneePerson.Create(cmd.OldName, cmd.OldPosition);
          var new_ = ConsigneePerson.Create(cmd.NewName, cmd.NewPosition);
          record.Load(events);
          record.updateConsignee(old, new_, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }
  }
}
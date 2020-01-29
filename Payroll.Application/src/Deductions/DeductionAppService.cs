using System;
using Payroll.Domain.Deductions;
using Payroll.EventSourcing;

namespace Payroll.Application.Deductions
{
  public class DeductionAppService : IDeductionAppService
  {
    private readonly IAccessTokenProvider _tokenProvider;
    private readonly IEventStore _eventStore;
    
    public DeductionAppService(IAccessTokenProvider tokenProvider, IEventStore eventStore)
    {
      _tokenProvider = tokenProvider;
      _eventStore = eventStore;
    }

    public void Handle(Contracts.V1.CreateDeduction cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        var record = Deduction.Create(Guid.NewGuid(), cmd.EmplyoeeId, cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
        record.setAmount(cmd.Amount, user.Id, DateTimeOffset.Now);
        record.setSchedule(cmd.Amortization, cmd.Schedule, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(Contracts.V1.CreateDeductionPayment cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Deduction>(cmd.DeductionId, out var events))
        {
          var record = new Deduction();
          record.Load(events);
          record.createPayment(cmd.Payment, cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.StopDeduction cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Deduction>(cmd.DeductionId, out var events))
        {
          var record = new Deduction();
          record.Load(events);
          record.StopDeduction(cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }
  }
}
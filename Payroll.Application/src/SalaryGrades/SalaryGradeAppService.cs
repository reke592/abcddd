using System;
using Payroll.Domain.SalaryGrades;
using Payroll.EventSourcing;

namespace Payroll.Application.SalaryGrades
{
  public class SalaryGradeAppService : ISalaryGradeAppService
  {
    private readonly IEventStore _eventStore;
    private readonly IAccessTokenProvider _tokenProvider;

    public SalaryGradeAppService(IAccessTokenProvider tokenProvider, IEventStore eventStore)
    {
      _eventStore = eventStore;
      _tokenProvider = tokenProvider;
    }

    public void Handle(Contracts.V1.CreateSalaryGrade cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        var record = SalaryGrade.Create(Guid.NewGuid(), cmd.BusinessYearId, cmd.GrossValue, user.UserId, DateTimeOffset.Now);
        // record.updateGross(cmd.GrossValue, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(Contracts.V1.UpdateSalaryGrade cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<SalaryGrade>(cmd.SalaryGradeId, out var events))
        {
          var record = new SalaryGrade();
          record.Load(events);
          record.updateGross(cmd.NewGrossValue, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }
  }
}
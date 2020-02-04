using System;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;

namespace Payroll.Application.Employees
{
  public class EmployeeAppService : IEmployeeAppService
  {
    private readonly IEventStore _eventStore;
    private readonly IAccessTokenProvider _tokenProvider;

    public EmployeeAppService(IAccessTokenProvider tokenProvider, IEventStore eventStore)
    {
      _tokenProvider = tokenProvider;
      _eventStore = eventStore;
    }

    public void Handle(Contracts.V1.CreateEmployee cmd, Action<EmployeeId> cb)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        var bioData = BioData.Create(cmd.Firstname, cmd.Middlename, cmd.Surname, Date.TryParse(cmd.DateOfBirth));
        var record = Employee.Create(Guid.NewGuid(), bioData, user.UserId, DateTimeOffset.Now);
        record.markEmployed(user.UserId, DateTimeOffset.Now);
        _eventStore.Save(record);
        cb(record.Id);
      });
    }

    public void Handle(Contracts.V1.UpdateBioData cmd) {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          var newBioData = BioData.Create(cmd.Firstname, cmd.Middlename, cmd.Surname, Date.TryParse(cmd.DateOfBirth));
          record.Load(events);
          record.updateBioData(newBioData, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.EmployEmployee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.markEmployed(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.SeparateEmployee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.markSeparated(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.GrantLeave cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.grantLeave(cmd.Start, cmd.Return, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.RevokeLeave cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.revokeLeave(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

      public void Handle(Contracts.V1.EndLeave cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.endLeave(user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.UpdateSalaryGrade cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Load(events);
          record.updateSalaryGrade(cmd.SalaryGradeId, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }
  }
}
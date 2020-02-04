using System;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.PayrollPeriods;
using Payroll.EventSourcing;

namespace Payroll.Application.PayrollPeriods
{
  public class PayrollPeriodAppService : IPayrollPeriodAppService
  {
    private readonly IEventStore _eventStore;
    private readonly IAccessTokenProvider _tokenProvider;

    public PayrollPeriodAppService(IAccessTokenProvider tokenProvider, IEventStore eventStore)
    {
      _eventStore = eventStore;
      _tokenProvider = tokenProvider;
    }

    public void Handle(Contracts.V1.CreatePayrollPeriod cmd, Action<PayrollPeriodId> cb)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        var record = PayrollPeriod.Create(Guid.NewGuid(), cmd.BusinessYearId, user.UserId, DateTimeOffset.Now);
        record.setApplicableMonth(cmd.ApplicableMonth, user.UserId, DateTimeOffset.Now);
        _eventStore.Save(record);
        cb(record.Id);
      });
    }

    public void Handle(Contracts.V1.AddPayrollConsignee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var consignee = ConsigneePerson.Create(cmd.Name, cmd.Position);
          record.Load(events);
          record.addConsignee(consignee, cmd.ConsigneeAction, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.RemovePayrollConsignee cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var consignee = PayrollConsignee.Create(cmd.Name, cmd.Position, cmd.ConsigneeAction);
          record.Load(events);
          record.removeConsignee(consignee, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.IncludeEmployeesToPayroll cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Load(events);
          foreach(var employee in cmd.EmployeeIds)
          {
            record.includeEmployee(employee, user.UserId, DateTimeOffset.Now);
          }
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.ExcludeEmployeesToPayroll cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Load(events);
          foreach(var employee in cmd.EmployeeIds)
          {
            record.excludeEmployee(employee, user.UserId, DateTimeOffset.Now);
          }
          _eventStore.Save(record);
        }
      });
    }

    // TODO: optimize, too slow for many adjustments
    public void Handle(Contracts.V1.AdjustPayrollDeductionPayment cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var adjustment = AdjustedDeductionPayment.Create(cmd.EmployeeId, cmd.DeductionId, cmd.AdjustedAmount);
          record.Load(events);
          record.adjustDeductionPayment(cmd.EmployeeId, adjustment, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(Contracts.V1.DispenseEmployeeSalary cmd)
    {
      _tokenProvider.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Load(events);
          record.giveOutSalary(cmd.EmployeeId, user.UserId, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }
  }
}
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using DeductionCommands = Payroll.Application.Deductions.Contracts.V1;
using EmployeeCommands = Payroll.Application.Employees.Contracts.V1;
using SalaryGradeCommands = Payroll.Application.SalaryGrades.Contracts.V1;
using PayrollCommands = Payroll.Application.PayrollPeriods.Contracts.V1;
using UserCommands = Payroll.Application.Users.Contracts.V1;
using Payroll.Domain.BusinessYears;
using System;
using Payroll.EventSourcing;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.Domain.Deductions;
using Payroll.Domain.SalaryGrades;
using Payroll.Domain.PayrollPeriods;

namespace Payroll.Application
{
  public class PayrollApplicationService
  {
    private readonly IAccessTokenProvider _tokenService;
    private readonly IEventStore _eventStore;

    public PayrollApplicationService(IAccessTokenProvider tokenService, IEventStore eventStore)
    {
      _tokenService = tokenService;
      _eventStore = eventStore;
    }

    //------------------------------------------
    #region BusinessYear Commands
    public void Handle(BusinessYearCommands.CreateBusinessYear cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = BusinessYear.Create(Guid.NewGuid(), user.Id, DateTimeOffset.Now);
        record.Start(cmd.ApplicableYear, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(BusinessYearCommands.EndBusinessYear cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          record.Apply(events);
          record.End(user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(BusinessYearCommands.CreateConsignee cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          record.Apply(events);
          record.addConsignee(ConsigneePerson.Create(cmd.Name, cmd.Position), user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(BusinessYearCommands.UpdateConsignee cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<BusinessYear>(cmd.BusinessYearId, out var events))
        {
          var record = new BusinessYear();
          var old = ConsigneePerson.Create(cmd.OldName, cmd.OldPosition);
          var new_ = ConsigneePerson.Create(cmd.NewName, cmd.NewPosition);
          record.Apply(events);
          record.updateConsignee(old, new_, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    #endregion
    
    //------------------------------------------
    #region Employee Commands
    public void Handle(EmployeeCommands.CreateEmployee cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = Employee.Create(Guid.NewGuid(), user.Id, DateTimeOffset.Now);
        var bioData = BioData.Create(cmd.Firstname, cmd.Middlename, cmd.Surname, Date.TryParse(cmd.DateOfBirth));
        record.updateBioData(bioData, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(EmployeeCommands.ChangeStatus cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmplyoeeId, out var events))
        {
          var record = new Employee();
          record.Apply(events);
          record.changeStatus(cmd.NewStatus, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(EmployeeCommands.UpdateSalaryGrade cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Employee>(cmd.EmployeeId, out var events))
        {
          var record = new Employee();
          record.Apply(events);
          record.updateSalaryGrade(cmd.SalaryGradeId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    #endregion

    //------------------------------------------
    #region Deduciton Commands
    public void Handle(DeductionCommands.CreateDeduction cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = Deduction.Create(Guid.NewGuid(), cmd.EmplyoeeId, cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
        record.setAmount(cmd.Amount, user.Id, DateTimeOffset.Now);
        record.setSchedule(cmd.Amortization, cmd.Schedule, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(DeductionCommands.CreateDeductionPayment cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Deduction>(cmd.DeductionId, out var events))
        {
          var record = new Deduction();
          record.Apply(events);
          record.createPayment(cmd.Payment, cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(DeductionCommands.StopDeduction cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<Deduction>(cmd.DeductionId, out var events))
        {
          var record = new Deduction();
          record.Apply(events);
          record.StopDeduction(cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    #endregion

    //------------------------------------------
    #region SalaryGrade Commands
    public void Handle(SalaryGradeCommands.CreateSalaryGrade cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = SalaryGrade.Create(Guid.NewGuid(), cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
        record.updateGross(cmd.GrossValue, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(SalaryGradeCommands.UpdateSalaryGrade cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<SalaryGrade>(cmd.SalaryGradeId, out var events))
        {
          var record = new SalaryGrade();
          record.Apply(events);
          _eventStore.Save(record);
        }
      });
    }

    #endregion
  
    //------------------------------------------
    #region PayrollPeriod Commands
    public void Handle(PayrollCommands.CreatePayrollPeriod cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        var record = PayrollPeriod.Create(Guid.NewGuid(), cmd.BusinessYearId, user.Id, DateTimeOffset.Now);
        record.setApplicableMonth(cmd.ApplicableMonth, user.Id, DateTimeOffset.Now);
        _eventStore.Save(record);
      });
    }

    public void Handle(PayrollCommands.AddPayrollConsignee cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var consignee = ConsigneePerson.Create(cmd.Name, cmd.Position);
          record.Apply(events);
          record.addConsignee(consignee, cmd.ConsigneeAction, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(PayrollCommands.RemovePayrollConsignee cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var consignee = PayrollConsignee.Create(cmd.Name, cmd.Position, cmd.ConsigneeAction);
          record.Apply(events);
          record.removeConsignee(consignee, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(PayrollCommands.IncludeEmployeesToPayroll cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Apply(events);
          foreach(var employee in cmd.EmployeeIds)
          {
            record.includeEmployee(employee, user.Id, DateTimeOffset.Now);
          }
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(PayrollCommands.ExcludeEmployeesToPayroll cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Apply(events);
          foreach(var employee in cmd.EmployeeIds)
          {
            record.excludeEmployee(employee, user.Id, DateTimeOffset.Now);
          }
          _eventStore.Save(record);
        }
      });
    }

    // TODO: optimize, too slow for many adjustments
    public void Handle(PayrollCommands.AdjustPayrollDeductionPayment cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          var adjustment = AdjustedDeductionPayment.Create(cmd.EmployeeId, cmd.DeductionId, cmd.AdjustedAmount);
          record.Apply(events);
          record.adjustDeductionPayment(cmd.EmployeeId, adjustment, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    public void Handle(PayrollCommands.DispenseEmployeeSalary cmd)
    {
      _tokenService.ReadToken(cmd.AccessToken, user => {
        if(_eventStore.TryGet<PayrollPeriod>(cmd.PayrollPeriodId, out var events))
        {
          var record = new PayrollPeriod();
          record.Apply(events);
          record.giveOutSalary(cmd.EmployeeId, user.Id, DateTimeOffset.Now);
          _eventStore.Save(record);
        }
      });
    }

    #endregion

  }
}
using System;
using System.Collections.Generic;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.Domain.Users;

namespace Payroll.Domain.PayrollPeriods
{
  public class PayrollPeriod : Aggregate
  {
    public BusinessYearId BusinessYear { get; private set; }
    public LongMonth ApplicableMonth { get; set; }
    // private IList<ConsigneePerson> _consigneeList = new List<ConsigneePerson>();
    private IList<PayrollConsignee> _consignees = new List<PayrollConsignee>();
    private IList<EmployeeId> _employees = new List<EmployeeId>();
    private IList<EmployeeId> _received = new List<EmployeeId>();
    private ISet<AdjustedDeductionPayment> _paymentsAdjusted = new HashSet<AdjustedDeductionPayment>();

    protected override void When(object e) {
      switch(e)
      {
        case Events.V1.PayrollPeriodCreated x:
          Id = x.Id;
          BusinessYear = x.BusinessYear;
          break;
        
        case Events.V1.PayrollPeriodApplicableMonthSettled x:
          ApplicableMonth = x.NewApplicableMonth;
          break;
        
        // TODO: clarify x.ConsigneeAction
        case Events.V1.PayrollPeriodConsigneeIncluded x:
          _consignees.Add(x.Consignee);
          break;

        case Events.V1.PayrollPeriodConsigneeRemoved x:
          _consignees.Remove(x.Consignee);
          break;
        
        case Events.V1.PayrollPeriodEmployeeIncluded x:
          _employees.Add(x.Employee);
          break;
        
        case Events.V1.PayrollPeriodEmployeeExcluded x:
          _employees.Remove(x.Employee);
          break;

        case Events.V1.PayrollPeriodDeductionPaymentAdjusted x:
          _paymentsAdjusted.Add(x.Adjusment);
          break;
        
        case Events.V1.PayrollPeriodEmployeeSalaryReceived x:
          _received.Add(x.Employee);
          break;
      }
    }

    public static PayrollPeriod Create(PayrollPeriodId id, BusinessYearId businessYear, UserId createdBy, DateTimeOffset createdAt)
    {
      var record = new PayrollPeriod();
      record.Apply(new Events.V1.PayrollPeriodCreated {
        Id = id,
        BusinessYear = businessYear,
        CreatedBy = createdBy,
        CreatedAt = createdAt
      });
      return record;
    }

    public void setApplicableMonth(LongMonth newApplicableMonth, UserId settledBy, DateTimeOffset settledAt)
    {
      if(_received.Count > 0)
        _updateFailed("can't change applicable month. salary already distributed", newApplicableMonth, settledBy, settledAt);
      else
        this.Apply(new Events.V1.PayrollPeriodApplicableMonthSettled {
          Id = this.Id,
          NewApplicableMonth = newApplicableMonth,
          SettledBy = settledBy,
          SettledAt = settledAt
        });
    }

    public void addConsignee(ConsigneePerson consignee, string consigneeAction, UserId includedBy, DateTimeOffset includedAt)
    {
      var person = PayrollConsignee.Create(consignee.Name, consignee.Position, consigneeAction);
      if(this._consignees.Contains(person))
        _updateFailed("can't include consignee. already included", consignee, includedBy, includedAt);
      else
        this.Apply(new Events.V1.PayrollPeriodConsigneeIncluded {
          Id = this.Id,
          Consignee = person,
          // ConsigneeAction = consigneeAction,
          IncludedBy = includedBy,
          IncludedAt = includedAt
        });
    }

    public void removeConsignee(PayrollConsignee consignee, UserId removedBy, DateTimeOffset removedAt)
    {
      if(!this._consignees.Contains(consignee))
        _updateFailed("can't remove consignee. not exist", consignee, removedBy, removedAt);
      else
        this.Apply(new Events.V1.PayrollPeriodConsigneeRemoved {
          Id = this.Id,
          Consignee = consignee,
          RemovedBy = removedBy,
          RemovedAt = removedAt
        });
    }

    public void includeEmployee(EmployeeId employee, UserId includedBy, DateTimeOffset includedAt)
    {
      if(this._employees.Contains(employee))
        _updateFailed("can't included employee. already included", employee, includedBy, includedAt);
      else
        this.Apply(new Events.V1.PayrollPeriodEmployeeIncluded {
          Id = this.Id,
          Employee = employee,
          IncludedBy = includedBy,
          IncludedAt = includedAt
        });
    }

    public void excludeEmployee(EmployeeId employee, UserId excludedBy, DateTimeOffset excludedAt)
    {
      if(this._employees.Contains(employee))
        _updateFailed("can't included employee. already excluded", employee, excludedBy, excludedAt);
      else
        this.Apply(new Events.V1.PayrollPeriodEmployeeExcluded {
          Id = this.Id,
          Employee = employee,
          ExcludedBy = excludedBy,
          ExcludedAt = excludedAt
        });
    }

    public void giveOutSalary(EmployeeId employee, UserId givenBy, DateTimeOffset givenAt)
    {
      if(this._received.Contains(employee))
        _updateFailed("can't dispense salary. already received", employee, givenBy, givenAt);
      else
        this.Apply(new Events.V1.PayrollPeriodEmployeeSalaryReceived {
          Id = this.Id,
          Employee = employee,
          GivenBy = givenBy,
          GivenAt = givenAt
        });
    }

    public void adjustDeductionPayment(EmployeeId employee, AdjustedDeductionPayment adjustment, UserId adjustedBy, DateTimeOffset adjustedAt)
    {
      if(!_employees.Contains(employee))
        _updateFailed("can't create adjustments. employee not included in payroll period", adjustment, adjustedBy, adjustedAt);
      else
        this.Apply(new Events.V1.PayrollPeriodDeductionPaymentAdjusted {
          Id = this.Id,
          Adjusment = adjustment,
          AdjustedBy = adjustedBy,
          AdjustedAt = adjustedAt
        });
    }

    private void _updateFailed(string reason, object value, UserId attemptedBy, DateTimeOffset attemptedAt)
      => this.Apply(new Events.V1.PayrollPeriodUpdateAttemptFailed
      {
        Id = this.Id,
        Reason = reason,
        AttemptedValue = value,
        AttemptedBy = attemptedBy,
        AttemptedAt = attemptedAt
      });
  }
}
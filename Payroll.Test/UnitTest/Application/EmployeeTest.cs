using System.Linq;
using Payroll.Domain.Employees;
using Xunit;
using Payroll.Application.Employees.Projections;
using EmployeeCommands = Payroll.Application.Employees.Contracts.V1;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using SalaryGradeCommands = Payroll.Application.SalaryGrades.Contracts.V1;
using Payroll.Domain.Shared;
using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using static Payroll.Application.SalaryGrades.Projections.SalaryGradeHistoryProjection;
using System;
using Payroll.EventSourcing.Serialization.YAML;
using Payroll.EventSourcing;
using System.Collections.Generic;
using static Payroll.Application.Employees.Projections.ActiveEmployeesProjection;
using static Payroll.Application.Employees.Projections.SeparatedEmployeesProjection;
using static Payroll.Application.Employees.Projections.EmployeesOnLeaveProjection;
using Payroll.Domain.BusinessYears;

namespace Payroll.Test.UnitTest.Application
{
  public class EmployeeTest : TestBase
  {
    private static int _stubCount = 0;

    private EmployeeId createStub()
    {
      EmployeeId stubId = null;
      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = $"Employee-{_stubCount++}",
        Middlename = "",
        Surname = "Stub",
        DateOfBirth = "1/1/2000"
      }, id => stubId = id);

      // var record = _cache.All<ActiveEmployeesProjection.ActiveEmployeeRecord>().Where(x => x.BioData.Firstname == $"Employee-{_stubCount++}").SingleOrDefault();
      // _cache.GetRecent<ActiveEmployeeRecord>(out var record);
      return stubId;
    }

    [Fact]
    public void CanCreateEmployee()
    {
      EmployeeId stubId;
      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = "Juan",
        Middlename = "",
        Surname = "Dela Cruz",
        DateOfBirth = "1/1/2000"
      }, id => stubId = id);

      var record = _cache.All<ActiveEmployeeRecord>().Where(x => x.BioData.Firstname == "Juan").SingleOrDefault();
      Assert.Equal("Juan", record.BioData.Firstname);
      _cache.Delete<ActiveEmployeeRecord>(record.EmployeeId);
    }

    [Fact]
    public void CanEmployEmployee()
    {
      var stub = createStub();
      
      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<ActiveEmployeeRecord>(stub);
      Assert.Equal(actual.Status, EmployeeStatus.EMPLOYED);
      _cache.Delete<ActiveEmployeeRecord>(stub);
    }

    [Fact]
    public void CanSeparateEmployee()
    {
      var stub = createStub();
      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var ee = _cache.Get<ActiveEmployeeRecord>(stub);
      Assert.Equal(EmployeeStatus.EMPLOYED, ee.Status);

      _app.Employee.Handle(new EmployeeCommands.SeparateEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var removed = _cache.Get<ActiveEmployeeRecord>(stub);
      Assert.Equal(null, removed);

      var actual = _cache.Get<SeparatedEmployeeRecord>(stub);
      Assert.Equal(EmployeeStatus.SEPARATED, actual.Status);
    }

    [Fact]
    public void CanGrantEmployeeLeave()
    {
      var stub = createStub();
      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      _app.Employee.Handle(new EmployeeCommands.GrantLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub,
        Start = Date.Now,
        Return = Date.Now.AddDays(2)
      });

      var actual = _cache.Get<OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, actual.Status);
      Assert.Equal($"Employee-{_stubCount - 1}", actual.BioData.Firstname);
    }

    [Fact]
    public void CanRevokeEmployeeLeave()
    {
      var stub = createStub();

      _app.Employee.Handle(new EmployeeCommands.GrantLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub,
        Start = Date.Now,
        Return = Date.Now.AddDays(2)
      });

      var ee_on_leave = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, ee_on_leave.Status);

      _app.Employee.Handle(new EmployeeCommands.RevokeLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<OnLeaveEmployeeRecord>(stub);
      Assert.Equal(null, actual);
    }

    [Fact]
    public void CanEndEmployeeLeave()
    {
      var stub = createStub();

      _app.Employee.Handle(new EmployeeCommands.GrantLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub,
        Start = Date.Now,
        Return = Date.Now.AddDays(2)
      });

      var ee_on_leave = _cache.Get<OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, ee_on_leave.Status);

      _app.Employee.Handle(new EmployeeCommands.EndLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<OnLeaveEmployeeRecord>(stub);
      Assert.Equal(null, actual);
    }

    [Fact]
    public void CanUpdateSalaryGrade()
    {
      BusinessYearId stubBusinessYearId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubBusinessYearId = id);

      // _cache.GetRecent<BusinessYearHistoryRecord>(out var year);
      var year = _cache.Get<BusinessYearHistoryRecord>(stubBusinessYearId);

      _app.BusinessYear.Handle(new BusinessYearCommands.StartBusinessYear {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.BusinessYearId
      });

      _app.SalaryGrade.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.BusinessYearId,
        GrossValue = 10000
      });

      var sg = _cache.All<SalaryGradeRecord>().Where(x => x.BusinessYear == year.Year).SingleOrDefault();

      var stub = createStub();

      _app.Employee.Handle(new EmployeeCommands.UpdateSalaryGrade {
        AccessToken = _accessTokenStub,
        EmployeeId = stub,
        SalaryGradeId = sg.SalaryGradeId
      });

      var actual = new Employee();
      _eventStore.TryGet<Employee>(stub, out var events);
      actual.Load(events);

      Assert.Equal<Guid>(sg.SalaryGradeId, actual.SalaryGrade);
    }
  }
}

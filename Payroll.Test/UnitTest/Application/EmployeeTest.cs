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

namespace Payroll.Test.UnitTest.Application
{
  public class EmployeeTest : TestBase
  {
    private static int _stubCount = 0;

    private EmployeeId createStub()
    {
      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = $"Employee-{_stubCount}",
        Middlename = "",
        Surname = "Stub",
        DateOfBirth = "1/1/2000"
      });

      var record = _cache.All<SeparatedEmployeesProjection.SeparatedEmployeeRecord>().Where(x => x.BioData.Firstname == $"Employee-{_stubCount++}").SingleOrDefault();
      return record.Id;
    }

    [Fact]
    public void CanCreateEmployee()
    {
      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = "Juan",
        Middlename = "",
        Surname = "Dela Cruz",
        DateOfBirth = "1/1/2000"
      });

      var record = _cache.All<SeparatedEmployeesProjection.SeparatedEmployeeRecord>().Where(x => x.BioData.Firstname == "Juan").SingleOrDefault();
      Assert.Equal("Juan", record.BioData.Firstname);
    }

    [Fact]
    public void CanEmployEmployee()
    {
      var stub = createStub();
      
      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<ActiveEmployeesProjection.ActiveEmployeeRecord>(stub);
      Assert.Equal(actual.Status, EmployeeStatus.EMPLOYED);
    }

    [Fact]
    public void CanSeparateEmployee()
    {
      var stub = createStub();
      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var ee = _cache.Get<ActiveEmployeesProjection.ActiveEmployeeRecord>(stub);
      Assert.Equal(EmployeeStatus.EMPLOYED, ee.Status);

      _app.Employee.Handle(new EmployeeCommands.SeparateEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var removed = _cache.Get<ActiveEmployeesProjection.ActiveEmployeeRecord>(stub);
      Assert.Equal(null, removed);

      var actual = _cache.Get<SeparatedEmployeesProjection.SeparatedEmployeeRecord>(stub);
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
        Return = Date.Create(2020, 2, 2)
      });

      var actual = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, actual.Status);
      Assert.Equal($"Employee-{_stubCount - 1}", actual.BioData.Firstname);
    }

    [Fact]
    public void CanRevokeEmployeeLeave()
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
        Return = Date.Create(2020, 2, 2)
      });

      var ee_on_leave = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, ee_on_leave.Status);

      _app.Employee.Handle(new EmployeeCommands.RevokeLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);
      Assert.Equal(null, actual);
    }

    [Fact]
    public void CanEndEmployeeLeave()
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
        Return = Date.Create(2020, 2, 2)
      });

      var ee_on_leave = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);

      Assert.Equal(EmployeeStatus.ON_LEAVE, ee_on_leave.Status);

      _app.Employee.Handle(new EmployeeCommands.EndLeave {
        AccessToken = _accessTokenStub,
        EmployeeId = stub
      });

      var actual = _cache.Get<EmployeesOnLeaveProjection.OnLeaveEmployeeRecord>(stub);
      Assert.Equal(null, actual);
    }

    [Fact]
    public void CanUpdateSalaryGrade()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var year = _cache.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();

      _app.BusinessYear.Handle(new BusinessYearCommands.StartBusinessYear {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.Id
      });

      _app.SalaryGrade.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.Id,
        GrossValue = 10000
      });

      var sg = _cache.All<SalaryGradeRecord>().Where(x => x.BusinessYear == year.Year).SingleOrDefault();

      var stub = createStub();

      _app.Employee.Handle(new EmployeeCommands.UpdateSalaryGrade {
        AccessToken = _accessTokenStub,
        EmployeeId = stub,
        SalaryGradeId = sg.Id
      });

      var actual = new Employee();
      _eventStore.TryGet<Employee>(stub, out var events);
      actual.Load(events);

      Assert.Equal<Guid>(sg.Id, actual.SalaryGrade);
    }
  }
}

using Payroll.Domain.BusinessYears;
using Payroll.Domain.Employees;
using static Payroll.Application.Employees.Projections.ActiveEmployeesProjection;
using EmployeeCommands = Payroll.Application.Employees.Contracts.V1;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using static Payroll.Application.BusinessYears.Projections.CurrentBusinessYearProjection;
using System;
using static Payroll.Application.Employees.Projections.SeparatedEmployeesProjection;

namespace Payroll.Test.UnitTest.Application
{
  public abstract class DeductionTestBase : TestBase
  {
    protected BusinessYearId _stubBusinessYear;
    protected EmployeeId _stubEmployee;

    public DeductionTestBase() : base()
    {
      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        DateOfBirth = "1/1/2000",
        Firstname = "Juan",
        Middlename = "Santos",
        Surname = "Dela Cruz"
      });

      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      // by default, newly created employees are separated
      _cache.GetRecent<SeparatedEmployeeRecord>(out var ee_created);

      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = ee_created.Id
      });

      _cache.GetRecent<ActiveEmployeeRecord>(out var ee);
      _cache.GetRecent<CurrentBusinessYearRecord>(out var year);
      _stubEmployee = ee.Id;
      _stubBusinessYear = year.Id;
    }

    public new void Dispose()
    {
      _stubBusinessYear = null;
      _stubEmployee = null;
      base.Dispose();
    }
  }
}
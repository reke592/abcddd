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
      EmployeeId stubEmpId = null;
      BusinessYearId stubBusinessYearId = null;

      _app.Employee.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        DateOfBirth = "1/1/2000",
        Firstname = "Juan",
        Middlename = "Santos",
        Surname = "Dela Cruz"
      }, id => stubEmpId = id);

      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubBusinessYearId = id);

      var ee_created = _cache.Get<ActiveEmployeeRecord>(stubEmpId);

      _app.Employee.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = ee_created.EmployeeId
      });

      // assign value to protected fields
      _stubEmployee = stubEmpId;
      _stubBusinessYear = stubBusinessYearId;
    }

    public new void Dispose()
    {
      _stubBusinessYear = null;
      _stubEmployee = null;
      base.Dispose();
    }
  }
}
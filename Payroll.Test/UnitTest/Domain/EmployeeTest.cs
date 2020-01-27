using System.Linq;
using Payroll.Domain.Employees;
using Xunit;
using Payroll.Application.Employees.Projections;
using EmployeeCommands = Payroll.Application.Employees.Contracts.V1;

namespace Payroll.Test.UnitTest.Domain
{
  public class EmployeeTest : TestBase
  {

    [Fact]
    public void CanAddEmployee()
    {
      _app.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = "Juan",
        Middlename = "",
        Surname = "Dela Cruz",
        DateOfBirth = "1/1/2000"
      });
    }

    [Fact]
    public void CanEmployEmployee() {
      _app.Handle(new EmployeeCommands.CreateEmployee {
        AccessToken = _accessTokenStub,
        Firstname = "Juan",
        Middlename = "",
        Surname = "Dela Cruz",
        DateOfBirth = "1/1/2000"
      });

      var record = _snapshots.All<SeparatedEmployeesProjection.SeparatedEmployeeRecord>().Where(x => x.BioData.Firstname == "Juan").SingleOrDefault();
      
      _app.Handle(new EmployeeCommands.EmployEmployee {
        AccessToken = _accessTokenStub,
        EmployeeId = record.Id
      });

      var actual = _snapshots.Get<ActiveEmployeesProjection.ActiveEmployeeRecord>(record.Id);
      Assert.Equal(actual.Status, EmployeeStatus.EMPLOYED);
    }
  }
}
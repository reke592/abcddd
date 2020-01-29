using Payroll.Domain.Deductions;
using Xunit;
using DeductionCommands = Payroll.Application.Deductions.Contracts.V1;

namespace Payroll.Test.UnitTest.Application
{
  public class DeductionTest : DeductionTestBase
  {
    [Fact]
    public void CanCreateDeduction()
    {
      _app.Deduction.Handle(new DeductionCommands.CreateDeduction {
        AccessToken = _accessTokenStub,
        Amortization = 3,
        Amount = 100,
        BusinessYearId = _stubBusinessYear,
        EmplyoeeId = _stubEmployee,
        Schedule = DeductionSchedule.TEMPORARY
      });
    }
  }
}
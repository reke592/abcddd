using Payroll.Domain.Deductions;
using Xunit;
using static Payroll.Application.Deductions.Projections.NonMandatoryDeductionProjection;
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
        Schedule = DeductionSchedule.NON_MANDATORY
      });
    }

    [Fact]
    public void CanCreatePayment()
    {
      _app.Deduction.Handle(new DeductionCommands.CreateDeduction {
        AccessToken = _accessTokenStub,
        Amortization = 3,
        Amount = 100,
        BusinessYearId = _stubBusinessYear,
        EmplyoeeId = _stubEmployee,
        Schedule = DeductionSchedule.NON_MANDATORY
      });

      _cache.GetRecent<NonMandatoryDeductionRecord>(out var doc);
      _app.Deduction.Handle(new DeductionCommands.CreateDeductionPayment {
        AccessToken = _accessTokenStub,
        BusinessYearId = _stubBusinessYear,
        DeductionId = doc.DeductionId,
        Payment = 50
      });

      _cache.GetRecent<NonMandatoryDeductionRecord>(out var actual);
      Assert.Equal(50, actual.TotalPaid);
      Assert.Equal((100 * 3) - 50, actual.Balance);
    }
  }
}
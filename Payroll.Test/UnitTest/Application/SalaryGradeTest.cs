using Payroll.Domain.SalaryGrades;
using SalaryGradeCommands = Payroll.Application.SalaryGrades.Contracts.V1;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using Xunit;
using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using System.Linq;
using static Payroll.Application.SalaryGrades.Projections.SalaryGradeHistoryProjection;
using Payroll.Domain.BusinessYears;

namespace Payroll.Test.UnitTest.Application
{
  public class SalaryGradeTest : TestBase
  {
    private BusinessYearHistoryRecord createBusinessYearStub(int year)
    {
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = year
      }, id => stubId = id);

      var record = _cache.Get<BusinessYearHistoryRecord>(stubId);
      return record;
    }

    [Fact]
    public void CanCreateSalaryGrade()
    {
      var yearStub = createBusinessYearStub(2020);
      _app.SalaryGrade.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = yearStub.BusinessYearId,
        GrossValue = 10000
      });

      var actual = _cache.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();
      Assert.Equal(10000, actual.Gross);
    }

    [Fact]
    public void CanUpdateSalaryGrade()
    {
      var yearStub = createBusinessYearStub(2020);
      _app.SalaryGrade.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = yearStub.BusinessYearId,
        GrossValue = 10000
      });

      var record = _cache.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();
      
      _app.SalaryGrade.Handle(new SalaryGradeCommands.UpdateSalaryGrade {
        AccessToken = _accessTokenStub,
        SalaryGradeId = record.SalaryGradeId,
        NewGrossValue = 15000
      });

      var actual = _cache.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();

      Assert.Equal(15000, actual.Gross);
    }
  }
}
using Payroll.Domain.SalaryGrades;
using SalaryGradeCommands = Payroll.Application.SalaryGrades.Contracts.V1;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using Xunit;
using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using System.Linq;
using static Payroll.Application.SalaryGrades.Projections.SalaryGradeHistoryProjection;

namespace Payroll.Test.UnitTest.Application
{
  public class SalaryGradeTest : TestBase
  {
    private BusinessYearHistoryRecord createBusinessYearStub(int year)
    {
      _app.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = year
      });

      var record = _snapshots.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();
      return record;
    }

    [Fact]
    public void CanCreateSalaryGrade()
    {
      var yearStub = createBusinessYearStub(2020);
      _app.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = yearStub.Id,
        GrossValue = 10000
      });

      var actual = _snapshots.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();
      Assert.Equal(10000, actual.Gross);
    }

    [Fact]
    public void CanUpdateSalaryGrade()
    {
      var yearStub = createBusinessYearStub(2020);
      _app.Handle(new SalaryGradeCommands.CreateSalaryGrade {
        AccessToken = _accessTokenStub,
        BusinessYearId = yearStub.Id,
        GrossValue = 10000
      });

      var record = _snapshots.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();
      
      _app.Handle(new SalaryGradeCommands.UpdateSalaryGrade {
        AccessToken = _accessTokenStub,
        SalaryGradeId = record.Id,
        NewGrossValue = 15000
      });

      var actual = _snapshots.All<SalaryGradeRecord>().Where(x => x.BusinessYear == yearStub.Year).SingleOrDefault();

      Assert.Equal(15000, actual.Gross);
    }
  }
}
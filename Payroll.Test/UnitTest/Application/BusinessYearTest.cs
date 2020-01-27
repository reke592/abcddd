using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using System.Linq;
using Xunit;
using Payroll.Domain.BusinessYears;

namespace Payroll.Test.UnitTest.Application
{
  public class BusinessYearTest : TestBase
  {
    [Fact]
    public void CanCreateBusinessYear()
    {
      _app.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var actual = _snapshots.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();

      Assert.Equal(2020, actual.Year);
    }

    [Fact]
    public void CanAddConsignee()
    {
      _app.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var yearStub = _snapshots.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();
      var consignee = ConsigneePerson.Create("camilla dela torre", "finance officer");
      _app.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = consignee.Name,
        Position = consignee.Position,
        BusinessYearId = yearStub.Id
      });

      var actual = _snapshots.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.True(actual.Consignees.Contains(consignee));
    }

    [Fact]
    public void CanUpdateConsignee()
    {
      _app.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var yearStub = _snapshots.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();
      var old = ConsigneePerson.Create("camilla dela torre", "finance officer");
      var new_ = ConsigneePerson.Create("juan felipe", "finance officer");

      _app.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = old.Name,
        Position = old.Position,
        BusinessYearId = yearStub.Id
      });

      var year = _snapshots.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.Contains<ConsigneePerson>(old, year.Consignees);

      _app.Handle(new BusinessYearCommands.UpdateConsignee {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.Id,
        OldName = old.Name,
        OldPosition = old.Position,
        NewName = new_.Name,
        NewPosition = new_.Position
      });

      var actual = _snapshots.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.False(actual.Consignees.Contains(old));
      Assert.True(actual.Consignees.Contains(new_));
      Assert.Equal(1, actual.Consignees.Count);
    }
  }
}
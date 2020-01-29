using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using System.Linq;
using Xunit;
using Payroll.Domain.BusinessYears;
using static Payroll.Application.BusinessYears.Projections.CurrentBusinessYearProjection;

namespace Payroll.Test.UnitTest.Application
{
  public class BusinessYearTest : TestBase
  {
    [Fact]
    public void CanCreateBusinessYear()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var actual = _cache.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();

      Assert.Equal(2020, actual.Year);
    }

    [Fact]
    public void CanAddConsignee()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var yearStub = _cache.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();
      var consignee = ConsigneePerson.Create("camilla dela torre", "finance officer");
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = consignee.Name,
        Position = consignee.Position,
        BusinessYearId = yearStub.Id
      });

      var actual = _cache.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.True(actual.Consignees.Contains(consignee));
    }

    [Fact]
    public void CanUpdateConsignee()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      var yearStub = _cache.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();
      var old = ConsigneePerson.Create("camilla dela torre", "finance officer");
      var new_ = ConsigneePerson.Create("juan felipe", "finance officer");

      _app.BusinessYear.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = old.Name,
        Position = old.Position,
        BusinessYearId = yearStub.Id
      });

      var year = _cache.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.Contains<ConsigneePerson>(old, year.Consignees);

      _app.BusinessYear.Handle(new BusinessYearCommands.UpdateConsignee {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.Id,
        OldName = old.Name,
        OldPosition = old.Position,
        NewName = new_.Name,
        NewPosition = new_.Position
      });

      var actual = _cache.Get<BusinessYearHistoryRecord>(yearStub.Id);

      Assert.False(actual.Consignees.Contains(old));
      Assert.True(actual.Consignees.Contains(new_));
      Assert.Equal(1, actual.Consignees.Count);
    }

    [Fact]
    public void CanStartBusinessYear()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      // GetRecent returns the last inserted in dictionary
      // we only have 1 record, since CurrentBusinessYearProjection is using a static Id
      // and it always reset the projections when a BusinessYear was created
      // TODO: add domain guards for BusinessYear.Create and BusinessYear.Start
      // we can create bussiness year
      // but we cannot start the new business year, if the current is not yet ended
      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var doc))
      {
        _app.BusinessYear.Handle(new BusinessYearCommands.StartBusinessYear {
          AccessToken = _accessTokenStub,
          BusinessYearId = doc.Id
        });
      }

      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var actual))
      {
        Assert.True(actual.Started);
      }
    }

    [Fact]
    public void CanEndCurrentBusinessYear()
    {
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      });

      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var doc)) {
        _app.BusinessYear.Handle(new BusinessYearCommands.StartBusinessYear {
          AccessToken = _accessTokenStub,
          BusinessYearId = doc.Id
        });
      }

      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var stub)) {
        Assert.True(stub.Started);

        _app.BusinessYear.Handle(new BusinessYearCommands.EndBusinessYear {
          AccessToken = _accessTokenStub,
          BusinessYearId = stub.Id
        });
      }

      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var actual)) {
        Assert.True(actual.Ended);
      }  
    }
  }
}
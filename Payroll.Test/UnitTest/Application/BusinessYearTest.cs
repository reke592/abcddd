using static Payroll.Application.BusinessYears.Projections.BusinessYearHistoryProjection;
using BusinessYearCommands = Payroll.Application.BusinessYears.Contracts.V1;
using System.Linq;
using Xunit;
using Payroll.Domain.BusinessYears;
using static Payroll.Application.BusinessYears.Projections.CurrentBusinessYearProjection;
using System;

namespace Payroll.Test.UnitTest.Application
{
  public class BusinessYearTest : TestBase
  {
    [Fact]
    public void CanCreateBusinessYear()
    {
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubId = id);

      var actual = _cache.All<BusinessYearHistoryRecord>().Where(x => !x.Ended).SingleOrDefault();

      Assert.Equal(2020, actual.Year);
    }

    [Fact]
    public void CanAddConsignee()
    {
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubId = id);

      var yearStub = _cache.Get<BusinessYearHistoryRecord>(stubId);
      var consignee = ConsigneePerson.Create("camilla dela torre", "finance officer");
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = consignee.Name,
        Position = consignee.Position,
        BusinessYearId = yearStub.BusinessYearId
      });

      var actual = _cache.Get<BusinessYearHistoryRecord>(yearStub.BusinessYearId);

      Assert.True(actual.Consignees.Contains(consignee));
    }

    [Fact]
    public void CanUpdateConsignee()
    {
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubId = id);

      var yearStub = _cache.Get<BusinessYearHistoryRecord>(stubId);
      var old = ConsigneePerson.Create("camilla dela torre", "finance officer");
      var new_ = ConsigneePerson.Create("juan felipe", "finance officer");

      _app.BusinessYear.Handle(new BusinessYearCommands.CreateConsignee {
        AccessToken = _accessTokenStub,
        Name = old.Name,
        Position = old.Position,
        BusinessYearId = yearStub.BusinessYearId
      });

      var year = _cache.Get<BusinessYearHistoryRecord>(yearStub.BusinessYearId);

      Assert.Contains<ConsigneePerson>(old, year.Consignees);

      _app.BusinessYear.Handle(new BusinessYearCommands.UpdateConsignee {
        AccessToken = _accessTokenStub,
        BusinessYearId = year.BusinessYearId,
        OldName = old.Name,
        OldPosition = old.Position,
        NewName = new_.Name,
        NewPosition = new_.Position
      });

      var actual = _cache.Get<BusinessYearHistoryRecord>(yearStub.BusinessYearId);

      Assert.False(actual.Consignees.Contains(old));
      Assert.True(actual.Consignees.Contains(new_));
      Assert.Equal(1, actual.Consignees.Count);
    }

    [Fact]
    public void CanStartBusinessYear()
    {
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubId = id);

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
          BusinessYearId = doc.BusinessYearId
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
      BusinessYearId stubId = null;
      _app.BusinessYear.Handle(new BusinessYearCommands.CreateBusinessYear {
        AccessToken = _accessTokenStub,
        ApplicableYear = 2020
      }, id => stubId = id);

      
      _app.BusinessYear.Handle(new BusinessYearCommands.StartBusinessYear {
        AccessToken = _accessTokenStub,
        BusinessYearId = stubId
      });
      
      var stubYear = _cache.Get<CurrentBusinessYearRecord>(stubId);
      
      Assert.True(stubYear.Started);
      _app.BusinessYear.Handle(new BusinessYearCommands.EndBusinessYear {
        AccessToken = _accessTokenStub,
        BusinessYearId = stubId
      });

      var actual = _cache.Get<CurrentBusinessYearRecord>(stubId);
      Assert.True(actual.Ended);
    }
  }
}
using System;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.EventSourcing;
using Xunit;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class UseEventStoreTest : BootstrapInjectTestBase
  {
    [Fact]
    public void CanSaveToEventStore()
    {
      var store = _container.Resolve<IEventStore>();
      var stubUserId = Guid.NewGuid();
      var stubEmpId = Guid.NewGuid();
      var stubBioData = BioData.Create("Juan", "Santos", "Dela Cruz", Date.TryParse("1/1/2000"));

      var employee = Employee.Create(stubEmpId, stubUserId, DateTimeOffset.Now);
      employee.updateBioData(stubBioData, stubUserId, DateTimeOffset.Now);
      employee.markEmployed(stubUserId, DateTimeOffset.Now);
      store.Save<Employee>(employee);
    }

    [Fact]
    public void CanLoadFromEventStore()
    {
      var store = _container.Resolve<IEventStore>();
      store.TryGet<Employee>(Guid.Parse("558c30b0-85ac-458c-b5e6-a5cdeca32272"), out var events);
      var employee = new Employee();
      employee.Load(events);
      var latest = store.LatestVersion<Employee>(employee);
      Assert.Equal(2, latest);
    }
  }
}
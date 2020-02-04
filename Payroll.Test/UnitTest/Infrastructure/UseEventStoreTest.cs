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

      var employee = Employee.Create(stubEmpId, stubBioData, stubUserId, DateTimeOffset.Now);
      store.Save<Employee>(employee);
    }

    [Fact]
    public void CanLoadFromEventStore()
    {
      var store = _container.Resolve<IEventStore>();
      store.TryGet<Employee>(Guid.Parse("bb4710ca-a42e-4bba-80eb-2e615ffbbc77"), out var events);
      var employee = new Employee();
      employee.Load(events);
      var latest = store.LatestVersion<Employee>(employee);
      Assert.Equal(1, latest);
    }
  }
}
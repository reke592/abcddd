using System;
using Payroll.Application.Employees.Projections;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.Test.UnitTest.Impl;
using Xunit;
using static Payroll.Application.Employees.Projections.ActiveEmployeesProjection;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class UseRavenDbTest
  {
    [Fact]
    public void CanStoreToDB()
    {
      var db = new UseRavenDb();
      var stubId = Guid.NewGuid();
      var stubOwner = Guid.NewGuid();
      var stubBioData = BioData.Create("Juan", "Santos", "Dela Cruz", Date.TryParse("1/1/2000"));
      var stubEvents = Employee.Create(stubId, stubBioData, stubOwner, DateTimeOffset.Now).Events;
      var projector = new ActiveEmployeesProjection();
      
      db.Start(new string[] { "http://localhost:8080" }, "payroll");
      foreach(var e in stubEvents)
      {
        projector.Handle(e, db);
      }
    }
  }
}
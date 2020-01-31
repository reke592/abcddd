using System;
using System.Text;
using Payroll.Domain.Employees;
using Payroll.Domain.Shared;
using Payroll.Domain.Users;
using Payroll.Test.UnitTest.Impl;
using Xunit;
using static Payroll.Domain.Employees.Events.V1;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class JsonSerializationTest
  {
    [Fact]
    public void CanSerializeGuid()
    {
      var guid = Guid.NewGuid();
      UserId userid = guid;
      var serializer = new JSONSerializer();
      // Console.WriteLine(ASCIIEncoding.UTF8.GetString(serializer.Serialize(guid)));
      Console.WriteLine(ASCIIEncoding.UTF8.GetString(serializer.Serialize(userid)));
    }

    [Fact]
    public void CanSerializeEventToJSON()
    {
      var serializer = new JSONSerializer();
      var stubUserId = Guid.NewGuid();
      var stubEmpId = Guid.NewGuid();
      var stubBioData = BioData.Create("Juan", "Santos", "Dela Cruz", Date.TryParse("1/1/2000"));

      var employee = Employee.Create(stubEmpId, stubUserId, DateTimeOffset.Now);
      employee.updateBioData(stubBioData, stubUserId, DateTimeOffset.Now);
      employee.markEmployed(stubUserId, DateTimeOffset.Now);
      
      var json = serializer.Serialize(employee.Events[2]);
      Console.WriteLine(ASCIIEncoding.UTF8.GetString(json));
    }

    [Fact]
    public void CanDeserializeEvent()
    {
      var serializer = new JSONSerializer();
      var stubUserId = Guid.NewGuid();
      var stubEmpId = Guid.NewGuid();
      var stubBioData = BioData.Create("Juan", "Santos", "Dela Cruz", Date.TryParse("1/1/2000"));

      var employee = Employee.Create(stubEmpId, stubUserId, DateTimeOffset.Now);
      employee.updateBioData(stubBioData, stubUserId, DateTimeOffset.Now);
      employee.markEmployed(stubUserId, DateTimeOffset.Now);
      
      var json = serializer.Serialize(employee.Events[1]);
      Console.WriteLine(ASCIIEncoding.UTF8.GetString(json));
      var actual = serializer.Deserialize(json, typeof(EmployeeBioDataUpdated));
    }
  }
}
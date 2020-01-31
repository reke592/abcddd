using System;
using System.Collections.Generic;
using Payroll.Domain.Employees;
using Payroll.EventSourcing;
using Payroll.EventSourcing.Serialization.YAML;
using Payroll.Test.UnitTest.Impl;
using Xunit;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class SerializationTest : TestBase
  {
    [Fact]
    public void CanSerializeDeserialize()
    {
      // Given one persisted event, root user
      var db = _eventStore as IYAMLSerializable;
      Console.WriteLine(db.YAML_Export());
      var actual = _serializer.Deserialize<IList<EventStreamPlaceholder>>(db.YAML_Export());

      Assert.True(actual.Count > 0);
      Assert.Equal("User Created", actual[0].Events[0].Name);
    }
  }
}
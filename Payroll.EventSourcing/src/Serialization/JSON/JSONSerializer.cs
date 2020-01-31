using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Payroll.Domain.BusinessYears;
using Payroll.Domain.Deductions;
using Payroll.Domain.Employees;
using Payroll.Domain.Users;
using Payroll.EventSourcing;

namespace Payroll.EventSourcing.Serialization.JSON
{
  public class JSONSerializer : ISerializer
  {
    public bool isJSON => true;
    private JsonSerializerOptions _options;

    public JSONSerializer()
    {
      _options = new JsonSerializerOptions();
      _options.Converters.Add(new AggregateIdJsonConverter<UserId>());
      _options.Converters.Add(new AggregateIdJsonConverter<EmployeeId>());
      _options.Converters.Add(new AggregateIdJsonConverter<BusinessYearId>());
      _options.Converters.Add(new AggregateIdJsonConverter<DeductionId>());
      _options.Converters.Add(new ValueObjectJsonDeserializer());
    }

    public object Deserialize(byte[] data, Type type) {
      // Console.WriteLine(ASCIIEncoding.UTF8.GetString(data));
      return JsonSerializer.Deserialize(data, type, _options);
    }

    public byte[] Serialize(object graph) {
      return ASCIIEncoding.UTF8.GetBytes(JsonSerializer.Serialize(graph, _options));
    }
  }
}
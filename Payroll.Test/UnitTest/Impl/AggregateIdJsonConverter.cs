using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Payroll.Domain;
using Payroll.Domain.Users;

namespace Payroll.Test.UnitTest.Impl
{
  public class AggregateIdJsonConverter<T> : JsonConverter<T> where T : AggregateId<T>
  {
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      // this will assume that Employee Id has a constructor with 1 parameter
      return (T) Activator.CreateInstance(typeof(T), Guid.Parse(reader.GetString()));
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
      writer.WriteStringValue(value.ToString());
    }
  }
}
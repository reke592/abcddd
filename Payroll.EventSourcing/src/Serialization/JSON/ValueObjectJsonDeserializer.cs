using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Payroll.Domain;

namespace Payroll.EventSourcing.Serialization.JSON
{
  public class ValueObjectJsonDeserializer : JsonConverterFactory
  {
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to
    /// </summary>
    public ValueObjectJsonDeserializer () { }

    public override bool CanConvert(Type typeToConvert) {
      return typeToConvert.IsSubclassOf(typeof(ValueObject));
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
      var type = typeof(ValueObjectConverter<>).MakeGenericType(typeToConvert);
      var converter = Activator.CreateInstance(type, new object[] { options });
      // var converter = Activator.CreateInstance(typeof(ValueObjectConverter<>)
      //   .MakeGenericType(typeToConvert)
      //   , BindingFlags.Instance | BindingFlags.Public
      //   , binder: null
      //   , args: new object[] { options }
      //   , culture: null);
      return (JsonConverter) converter;
    }

    private class ValueObjectConverter<T> : JsonConverter<T>
    {
      // private readonly JsonConverter<T> _valueConverter;
      private Type _targetType;

      public ValueObjectConverter(JsonSerializerOptions options)
      {
        // _valueConverter = (JsonConverter<T>) options.GetConverter(typeof(T));
        _targetType = typeof(T);
      }

      public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var record = Activator.CreateInstance<T>();
        PropertyInfo prop = null;
        
        if(reader.TokenType != JsonTokenType.StartObject)
          throw new JsonException();

        while(reader.Read())
        {
          if(reader.TokenType == JsonTokenType.EndObject)
            return record;

          if(reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();

          prop = typeof(T).GetProperty(reader.GetString());
          prop.SetValue(record, JsonSerializer.Deserialize(ref reader, prop.PropertyType, options));
        }
        throw new JsonException();
      }

      public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        // Console.WriteLine(typeof(T));
        writer.WriteStartObject();
        foreach(var prop in value.GetType().GetProperties())
        {
          writer.WritePropertyName(prop.Name);
          JsonSerializer.Serialize(writer, prop.GetValue(value), options);
        }
        writer.WriteEndObject();
        // var converter = (JsonConverter<T>) options.GetConverter(typeof(T));
        // converter.Write(writer, value, options);
      }
      
    }
  }
}

using System;
using System.Text;
using Payroll.EventSourcing;
using YamlDotNet.Serialization.NodeDeserializers;
using YAML = YamlDotNet.Serialization;

namespace Payroll.Test.UnitTest.Impl
{
  public class YAMLSerializer : ISerializer
  {
    public bool isJSON => false;
    private YAML.ISerializer _serializer;
    private YAML.IDeserializer _deserializer;
    private ITypeMapper _mapper;
    
    public YAMLSerializer(ITypeMapper mapper)
    {
      _mapper = mapper;
      _serializer = new YAML.SerializerBuilder()
        .WithTypeConverter(new DateTimeOffsetConverter(true))
        .Build();

      _deserializer = new YAML.DeserializerBuilder()
        .WithTypeConverter(new DateTimeOffsetConverter(true))
        .WithNodeDeserializer(
          inner => new EventNodeDeserializer(inner, _mapper),
          s => s.InsteadOf<ObjectNodeDeserializer>())
        .Build();
    }

    public object Deserialize(byte[] data, Type type) {
      return _deserializer.Deserialize(ASCIIEncoding.UTF8.GetString(data), type);
    }

    public byte[] Serialize(object graph) {
      return ASCIIEncoding.UTF8.GetBytes(_serializer.Serialize(graph));
    }
  }
}
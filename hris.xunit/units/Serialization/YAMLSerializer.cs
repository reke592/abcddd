using System.IO;
using hris.xunit.units.EventSourcing;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace hris.xunit.units.Serialization {
    public class YAMLSerializer : IYAMLSerializer
    {
        private static ISerializer _serializer;
        private static IDeserializer _deserializer;
        private ITypeMapper _mapper;

        public YAMLSerializer(ITypeMapper mapper)
        {
            _mapper = mapper;
            _serializer = new SerializerBuilder()
                        .WithTypeConverter(new DateTimeOffsetConverter(true))
                        .Build();
            _deserializer = new DeserializerBuilder()
                        .WithTypeConverter(new DateTimeOffsetConverter(true))
                        .WithNodeDeserializer(inner => new EventNodeDeserializer(inner, _mapper),
                                            s => s.InsteadOf<ObjectNodeDeserializer>())
                        .Build();
        }

        public string Serialize(object o)
        {
            return _serializer.Serialize(o);
        }

        public T Deserialize<T>(string raw)
        {
            return _deserializer.Deserialize<T>(raw);
        }
    }
}
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace hris.xunit.units.Serialization {
    public class YAMLSerializer
    {
        private static Serializer _serializer;
        private static IDeserializer _deserializer;

        public static Serializer GetSerializer {
            get {
                return _serializer ?? (_serializer = new Serializer());
            }
        }

        public static IDeserializer GetDeserializer {
            get {
                return _deserializer ?? (_deserializer = new DeserializerBuilder()
                                        .IgnoreUnmatchedProperties()
                                        // .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                        .Build());
            }
        }

        public string Serialize(object o)
        {
            return GetSerializer.Serialize(o);
        }
        
        public object Deserialize(string str)
        {
            return GetDeserializer.Deserialize(new StringReader(str));
        }
    }
}
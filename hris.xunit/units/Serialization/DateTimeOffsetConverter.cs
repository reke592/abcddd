using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace hris.xunit.units.Serialization
{
    public class DateTimeOffsetConverter : IYamlTypeConverter
    {
        private readonly bool jsonCompatible;

        public DateTimeOffsetConverter(bool jsonCompatible)
        {
            this.jsonCompatible = jsonCompatible;
        }

        public bool Accepts(Type type)
        {
            return type == typeof(DateTimeOffset);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;
            return DateTimeOffset.Parse(value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var dt = (DateTimeOffset) value;
            emitter.Emit(new Scalar(null, null, dt.ToString(), jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
        }
    }
}
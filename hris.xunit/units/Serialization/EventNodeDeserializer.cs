using System;
using System.Collections.Generic;
using System.Reflection;
using hris.xunit.units.EventSourcing;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace hris.xunit.units.Serialization
{
    public class EventNodeDeserializer : INodeDeserializer
    {
        ITypeMapper _mapper;
        private readonly INodeDeserializer _nodeDeserializer;

        public EventNodeDeserializer(INodeDeserializer nodeDeserializer, ITypeMapper mapper)
        {
            _nodeDeserializer = nodeDeserializer;
            _mapper = mapper;
        }

        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            if(_nodeDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value))
            {
                if(expectedType == typeof(EventSourcing.Event))
                {
                    var x = value as EventSourcing.Event;
                    var raw_meta = x.Metadata as Dictionary<object, object>;
                    var meta_type = _mapper.GetEventType(x.Name);
                    var meta = Activator.CreateInstance(meta_type);
                    foreach(var key in raw_meta.Keys)
                    {
                        var prop = meta_type.GetProperty(key.ToString());
                        prop.SetValue(meta, Extract(prop.PropertyType, raw_meta[key]));
                    }
                    value = new Event(x.Name, meta);
                }
                return true;
            }
            return false;
        }

        object Extract(Type type, object raw_value)
        {
            if(type == typeof(Guid))
            {
                return Guid.Parse(raw_value.ToString());
            }
            else if(type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(raw_value.ToString());
            }
            else if(type.IsSubclassOf(typeof(Enum)))
            {
                return Enum.Parse(type, raw_value.ToString());
            }
            else if(raw_value.GetType() == typeof(Dictionary<object, object>))
            {
                var meta = raw_value as Dictionary<object, object>;
                Console.WriteLine(type);
                var value_object = Activator.CreateInstance(type);
                foreach(var key in meta.Keys)
                {
                    var inner_type = type.GetField(key.ToString(), BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
                    Console.WriteLine(inner_type);
                    inner_type.SetValue(value_object, Extract(inner_type.FieldType, meta[key]));
                }
                return value_object;
            }
            else
            {
                return raw_value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Payroll.EventSourcing.Serialization
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
                    var meta_flags = BindingFlags.Instance 
                                    // | BindingFlags.DeclaredOnly 
                                    | BindingFlags.Public 
                                    | BindingFlags.NonPublic;
                    var meta_props = meta_type.GetProperties(meta_flags);
                    var meta = Activator.CreateInstance(meta_type);
                    // foreach(var key in raw_meta.Keys)
                    foreach(var prop in meta_props)
                    {
                        // var prop = meta_type.GetProperty(key.ToString());
                        if(raw_meta.TryGetValue(prop.Name, out var raw_value))
                        {
                            prop.SetValue(meta, Extract(prop.PropertyType, raw_value));
                        }
                    }
                    value = new Event(x.Name, meta);
                }
                return true;
            }
            return false;
        }

        object Extract(Type value_type, object raw_value)
        {
            if(value_type == typeof(Guid))
            {
                return Guid.Parse(raw_value.ToString());
            }
            else if(value_type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(raw_value.ToString());
            }
            else if(value_type.IsSubclassOf(typeof(Enum)))
            {
                return Enum.Parse(value_type, raw_value.ToString());
            }
            else if(raw_value.GetType() == typeof(Dictionary<object, object>))
            {
                var meta = raw_value as Dictionary<object, object>;
                // Console.WriteLine(type);
                var instance = Activator.CreateInstance(value_type);
                var binding_flags = BindingFlags.Instance 
                                    // | BindingFlags.DeclaredOnly 
                                    | BindingFlags.Public 
                                    | BindingFlags.NonPublic;
                // foreach(var key in meta.Keys)
                foreach(var prop in value_type.GetFields(binding_flags))
                {
                    // var field = value_type.GetField(key.ToString(), binding_flags);
                    // Console.WriteLine(inner_type);
                    if(meta.TryGetValue(prop.Name, out var value)){
                        prop.SetValue(instance, Extract(prop.FieldType, value));
                    }
                    // field.SetValue(instance, Extract(field.FieldType, meta[key]));
                }
                return instance;
            }
            else
            {
                return raw_value;
            }
        }
    }
}
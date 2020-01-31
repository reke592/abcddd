using System;
using System.Collections.Generic;
using System.Reflection;
using Payroll.EventSourcing;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Payroll.Test.UnitTest.Impl
{
    public class EventNodeDeserializer : INodeDeserializer
    {
        private readonly ITypeMapper _mapper;
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
                    value = Event.Create(x.StartLocation, x.Index, x.Name, meta);
                }
                return true;
            }
            return false;
        }

        object Extract(Type value_type, object raw_value)
        {
            if(value_type == typeof(Guid))
            return Guid.Parse(raw_value.ToString());
            
            else if(value_type == typeof(DateTimeOffset))
            return DateTimeOffset.Parse(raw_value.ToString());
            
            else if(value_type == typeof(Int16))
            return Int16.Parse(raw_value.ToString());
            
            else if(value_type == typeof(Int32))
            return Int32.Parse(raw_value.ToString());
            
            else if(value_type == typeof(Int64))
            return Int64.Parse(raw_value.ToString());

            else if(value_type == typeof(Decimal))
            return Decimal.Parse(raw_value.ToString());

            else if(value_type == typeof(Double))
            return Double.Parse(raw_value.ToString());

            else if(value_type == typeof(Boolean))
            return Boolean.Parse(raw_value.ToString());
            
            else if(value_type.IsSubclassOf(typeof(Enum)))
            return Enum.Parse(value_type, raw_value.ToString());

            else if(raw_value.GetType() == typeof(Dictionary<object, object>))
            {
                var meta = raw_value as Dictionary<object, object>;
                var instance = Activator.CreateInstance(value_type);
                var binding_flags = BindingFlags.Instance 
                                    | BindingFlags.Public 
                                    | BindingFlags.NonPublic;
                foreach(var prop in value_type.GetFields(binding_flags))
                {
                    if(meta.TryGetValue(prop.Name, out var value)){
                        prop.SetValue(instance, Extract(prop.FieldType, value));
                    }
                }

                foreach(var prop in value_type.GetProperties(binding_flags))
                {
                    if(meta.TryGetValue(prop.Name, out var value)){
                        prop.SetValue(instance, Extract(prop.PropertyType, value));
                    }
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
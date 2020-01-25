using System;
using System.Linq;
using System.Collections.Generic;

namespace Payroll.EventSourcing
{
    /// <summary>
    /// Avoid using a fully qualified package name for event name.
    /// Map the event class to a certain string event name.
    /// This can make us use any event class to handle a certain event in stream.
    /// </summary>
    public class TypeMapper : ITypeMapper
    {
        private IDictionary<string, Type>  _types = new Dictionary<string, Type>();
        public IEnumerable<string> RegisteredKeys => _types.Keys;
        
        public ITypeMapper Map<T>(string eventName)
        {
            _types.Add(eventName, typeof(T));
            return this;
        }

        public Type GetEventType(string eventName) {
            if(_types.TryGetValue(eventName, out var type))
            {
                return type;
            }
            throw new Exception($"Can't resolve event type for event: {eventName}");
        }

        public string GetEventName(object meta) {
            return GetEventName(meta.GetType());
        }

        public string GetEventName(Type event_type)
        {
            var key = _types.Where(x => x.Value == event_type).FirstOrDefault().Key;
            
            if(key is null)
                throw new Exception($"Can't resolve event name for event: {event_type}");
            // else
            return key;
        }

        public Type GetEventType(object meta)
        {
            var type = _types.Where(x => x.Value == meta.GetType()).FirstOrDefault().Value;
            
            if(type is null)
                throw new Exception($"Can't resolve event type for metadata: {meta.GetType()}");
            // else
            return type;
        }

    }
}
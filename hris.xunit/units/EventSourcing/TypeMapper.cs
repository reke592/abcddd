using System;
using System.Linq;
using System.Collections.Generic;

namespace hris.xunit.units.EventSourcing
{
    /// <summary>
    /// Avoid using a fully qualified package name for event name.
    /// Map the event class to a certain string event name.
    /// This can make us use any event class to handle a certain event in stream.
    /// </summary>
    public class TypeMapper
    {
        private IDictionary<string, Type>  _types = new Dictionary<string, Type>();
        
        public void Map<T>(string eventName)
        {
            _types.Add(eventName, typeof(T));
        }

        /// <summary>
        /// domain event type resolution
        /// </summary>
        public Type GetEventType(string eventName) {
            if(_types.TryGetValue(eventName, out var type))
            {
                return type;
            }
            throw new Exception($"Can't resolve event type for event: {eventName}");
        }

        /// <summary>
        /// event type to string
        /// </summary>
        public string GetEventName(object meta) {
            var key = _types.Where(x => x.Value == meta.GetType()).FirstOrDefault().Key;
            
            if(key is null)
                throw new Exception($"Can't resolve event name for event: {meta.GetType()}");
            // else
            return key;
        }
    }
}
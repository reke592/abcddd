using System;
using System.Collections.Generic;

namespace hris.xunit.units.EventSourcing
{
    /// <summary>
    /// Avoid using a fully qualified package name for event name.
    /// Map the event class to a certain string event name.
    /// This can make us use any event class to handle a certain event in stream.
    /// </summary>
    public static class TypeMapper
    {
        static IDictionary<string, Type>  _types = new Dictionary<string, Type>();
        
        static void Map<T>(string eventName)
        {
            _types.Add(eventName, typeof(T));
        }

        static Type TryEventType(string eventName) {
            _types.TryGetValue(eventName, out var type);
            return type;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Payroll.EventSourcing
{
    public interface ITypeMapper
    {
        // given a string name we can resolve an event type
        // acts like a context mapping for a given event
        Type GetEventType(string name);

        // given a type we can resolve an event name
        // a standard value that can be interpreted by other subdomain / bounded context
        // for context mapping
        string GetEventName(Type t);

        // given a boxed instance of event metadata we can resolve the event type
        // acts like a guard before storing an event to make sure we can make projections
        Type GetEventType(object meta);

        // given a boxed instance of event metadata we can resolve the event name
        // a syntactic sugar for event name when storing the event metadata
        string GetEventName(object meta);

        // we bind the event name to the current version of event type we use
        // we don't use the fully qualified name of a type to avoid getting tied forever to its namespace
        // To get the previous version of projection:
        //   we use the eventName to query for thesame event that happened before
        //   if the result is empty, we simply set the value to the empty representation of that value object
        void Map<T>(string eventName);

        IEnumerable<string> RegisteredKeys { get; }
    }
}
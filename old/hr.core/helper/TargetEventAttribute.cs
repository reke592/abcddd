using System;

namespace hr.core.helper {
    [AttributeUsage(AttributeTargets.Method)]
    public class TargetEventAttribute : Attribute {
        public Type EventType { get; private set; }

        public TargetEventAttribute(Type event_) {
            if(!event_.IsSubclassOf(typeof(Event)))
                throw new InvalidCastException($"Type ${event_} is not a subclass of ${typeof(Event)}");
            EventType = event_;
        }
    }
}
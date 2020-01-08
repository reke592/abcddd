using System;

namespace hr.core.helper {
    [AttributeUsage(AttributeTargets.Method)]
    public class TargetCommandAttribute : Attribute {
        public Type CommandType { get; private set; }

        public TargetCommandAttribute(Type command_) {
            if(!command_.IsSubclassOf(typeof(Command)))
                throw new InvalidCastException($"Type ${command_} is not a subclass of ${typeof(Command)}");
            CommandType = command_;
        }
    }
}
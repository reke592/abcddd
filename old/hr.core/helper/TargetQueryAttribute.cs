using System;

namespace hr.core.helper {
    [AttributeUsage(AttributeTargets.Method)]
    public class TargetQueryAttribute : Attribute {
        public Type QueryType { get; private set; }

        public TargetQueryAttribute(Type query_) {
            if(!query_.IsSubclassOf(typeof(Query)))
                throw new InvalidCastException($"Type ${query_} is not a subclass of ${typeof(Query)}");
            QueryType = query_;
        }
    }
}
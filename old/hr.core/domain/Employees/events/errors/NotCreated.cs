using System;

namespace hr.core.domain.Employees.events.errors {
    public class NotCreated : helper.Event {
        public object Data { get; private set; }
        public Type TypeData { get; private set; }
        public string Message { get; private set; }
        
        public NotCreated(object data, string message = null) {
            TypeData = data.GetType();
            Data = data;
            Message = message ?? $"Can't create {TypeData.Name}";
        }

        public T Cast<T>() {
            return (T) Data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace hr.core.helper {
    public abstract class BaseHandler {
        protected readonly static EventBroker Broker = EventBroker.getInstance();
        private readonly static BindingFlags _method_flags 
            = BindingFlags.Instance
            | BindingFlags.DeclaredOnly
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        // static registry for multiple same object instance
        private static IDictionary<Type, IDictionary<Type, MethodInfo>> _static_registry 
            = new Dictionary<Type, IDictionary<Type, MethodInfo>>();

        // handlers declared in child class
        private IDictionary<Type, MethodInfo> _handlers {
            get {
                var T = this.GetType();
                if(!_static_registry.ContainsKey(T))
                    _static_registry.Add(T, new Dictionary<Type, MethodInfo>());

                return _static_registry[T];
            }
        }
        
        public ReadOnlyDictionary<Type, MethodInfo> RegisteredHandlers {
            get {
                return new ReadOnlyDictionary<Type, MethodInfo>(_handlers);
            }
        }

        // look for TargetCommand | TargetEvent | TargetQuery methods attribute
        // register the method in static_registry
        private void _parse_attributes() {
            // Console.WriteLine($"parsing attribs: {this.GetType().Name}");
            var T = this.GetType();
            foreach(var method in T.GetMethods(_method_flags)) {
                foreach(var attr in method.GetCustomAttributes<TargetEventAttribute>())
                    _register_method(attr.EventType, method);
                foreach(var attr in method.GetCustomAttributes<TargetCommandAttribute>())
                    _register_method(attr.CommandType, method);
                foreach(var attr in method.GetCustomAttributes<TargetQueryAttribute>())
                    _register_method(attr.QueryType, method);
            }
        }

        private void _register_method(Type target_cast, MethodInfo method) {
            var param = method.GetParameters();
            if(param.Length != 2)
                throw new TargetParameterCountException($"Method must have only 2 parameters, (object sender, {target_cast.FullName} e)");
            
            if(param[1].ParameterType != target_cast)
                throw new InvalidCastException($"Can't cast from {param[1].ParameterType.FullName} to {target_cast.FullName}");

            // Console.WriteLine($"registered {this} handle for {target_cast}.");
            _handlers.Add(target_cast, method);
        }

        private void _on_command(object sender, Command c) {
            _cast<Command>(sender, c);
        }

        private void _on_query(object sender, Query q) {
            _cast<Query>(sender, q);
        }

        private void _on_event(object sender, Event e) {
            _cast<Event>(sender, e);
        }

        private void _cast<T>(object sender, T emitted) {

            var actual_type = emitted.GetType();
            var instance = this;
            var args = Convert.ChangeType(emitted, actual_type);
            
            // for handlers that target the actual type
            if(_handlers.ContainsKey(actual_type)) {
                _handlers[actual_type].Invoke(instance, new object[] { sender, args });
            }
            // for handlers that target the event inheritance, (eg. for integration event / error event)
            else if (_handlers.ContainsKey(actual_type.BaseType)) {
                _handlers[actual_type.BaseType].Invoke(instance, new object[] { sender, args });
            }
        }

        public BaseHandler() {
            _parse_attributes();
            Broker.addCommandListener(_on_command);
            Broker.addQueryListener(_on_query);
            Broker.addEventListener(_on_event);
        }

        ~BaseHandler() {
            Broker.removeCommandListener(_on_command);
            Broker.removeQueryListener(_on_query);
            Broker.removeEventListener(_on_event);
        }
    }
}
using System;
using System.Collections.Generic;

namespace hr.com.helper.domain {
    public class EventBroker {
        private static EventBroker _instance;
        private IList<Event> AllEvents;
        event EventHandler<Command> Commands;
        event EventHandler<Query> Queries;
        event EventHandler<Event> Events;

        public static EventBroker getInstance() {
            return _instance ?? (_instance = new EventBroker());
        }

        public void MapEvents(Action<Event> fn) {
            foreach(var e in this.AllEvents) {
                fn(e);
            }
        }

        private EventBroker() {
            this.AllEvents = new List<Event>();
        }

        public void Command(Command c) {
            this.Commands?.Invoke(this, c);
        }

        public T Query<T>(Query q) {
            this.Queries?.Invoke(this, q);
            // unboxing the result
            return (T) q.Result; 
        }

        public void Emit(Event e) {
            this.Events?.Invoke(this, e);
            if(e.Register)
                this.AllEvents.Add(e);
        }

        public void addCommandListener(EventHandler<Command> c) {
            this.Commands += c;
        }

        public void addQueryListener(EventHandler<Query> q) {
            this.Queries += q;
        }

        public void addEventListener(EventHandler<Event> e) {
            this.Events += e;
        }
    }

    public class Command : EventArgs {
    }

    public class Query {
        // boxing the result
        public object Result;
    }

}
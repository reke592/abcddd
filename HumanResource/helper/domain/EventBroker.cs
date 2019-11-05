using System;
using System.Collections.Generic;
using hr.domain;

namespace hr.helper.domain {
    public class EventBroker {
        private static EventBroker _instance;
        IList<Event> AllEvents;
        event EventHandler<Command> Commands;
        event EventHandler<Query> Queries;
        public static EventBroker getInstance {
            get {
                return _instance ?? (_instance = new EventBroker());
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

        public void addCommandListener(EventHandler<Command> c) {
            this.Commands += c;
        }

        public void addQueryListener(EventHandler<Query> q) {
            this.Queries += q;
        }
    }

    public class Command : EventArgs {
    }

    public class Query {
        // boxing the result
        public object Result;
    }

}
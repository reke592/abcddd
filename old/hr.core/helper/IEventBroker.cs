using System;

namespace hr.core.helper {
    public interface IEventBroker<TEvent, TCommand, TQuery> {
        void MapEvents(Action<TEvent> fn);
        void Emit(TEvent e);
        void Command(TCommand c);
        T Query<T>(TQuery q);
        void addEventListener(EventHandler<TEvent> handler);
        void addCommandListener(EventHandler<TCommand> handler);
        void addQueryListener(EventHandler<TQuery> handler);
        void removeEventListener(EventHandler<TEvent> handler);
        void removeCommandListener(EventHandler<TCommand> handler);
        void removeQueryListener(EventHandler<TQuery> handler);
    }
}
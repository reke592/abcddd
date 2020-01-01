namespace hr.core.helper {
    public abstract class Event {
        public bool Register = true;
    }

    public abstract class Event<T> : Event {

    }
}
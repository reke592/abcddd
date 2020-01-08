namespace hr.core.helper {
    public abstract class ErrorEvent : Event { 
        public ErrorEvent() {
            Register = false;
        }
    }
}
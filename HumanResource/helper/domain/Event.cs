using System;

namespace hr.helper.domain {
    public abstract class Event {
        public bool Register = true;
        public object Value = null;
    }
}
using hr.core.helper;

namespace console.com {
    public class TestEventA : Event {
        public string Value { get; private set; }

        public TestEventA(string value) {
            Value = value;
        }
    }
}
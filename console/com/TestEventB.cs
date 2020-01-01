using hr.core.helper;

namespace console.com {
    public class TestEventB : Event {
        public int SomeNumber { get; private set; }

        public TestEventB(int value) {
            SomeNumber = value;
        }
    }
}
using hr.helper.domain;

namespace hr.helper.errors {
    public class Errored : Command {
        public object Value { get; private set; }

        public Errored(object annonymous) {
            Value = annonymous;
        }
    }
}
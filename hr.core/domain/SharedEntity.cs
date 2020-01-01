namespace hr.core.domain {
    public abstract class SharedEntity : ValueObject {
        public virtual long Id { get; protected set; }
    }
}
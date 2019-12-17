namespace hr.com.domain {
    // not nullable T Id
    public abstract class ValueObject<T> where T : struct {
        public virtual T Id { get; protected set; }
        public virtual object Actual => this;

        public override bool Equals(object obj) {
            var other = obj as ValueObject<T>;

            if (other is null)
            return false;

            if (ReferenceEquals(this, other))
            return true;

            if (Actual.GetType() != other.GetType())
            return false;

            // if (Id.ToString().Equals(other.ToString()))
            // return true;
            
            return string.Equals(Id.ToString(), other.Id.ToString());
        }

        public override int GetHashCode() {
            return (Actual.GetType().ToString() + Id.ToString()).GetHashCode();
        }
    }
}
namespace hr.core.domain {
    /// <summary>
    /// base class for all domain entities. 
    /// https://enterprisecraftsmanship.com/posts/entity-base-class/
    /// </summary>
    public abstract class Entity {
        // version Id is a persistence layer concern
        public virtual long Id { get; protected set; }
        public virtual object Actual => this;

        public override bool Equals(object obj) {
            var other = obj as Entity;

            if (other is null)
            return false;

            if (ReferenceEquals(this, other))
            return true;

            if (Actual.GetType() != other.GetType())
            return false;

            // because transient objects have the default Id value of 0
            if (Id == 0 || other.Id == 0)
            return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b) {
            if (a is null && b is null)
            return true;
            if (a is null || b is null)
            return false;
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            return (Actual.GetType().ToString() + Id).GetHashCode();
        }
    }
}
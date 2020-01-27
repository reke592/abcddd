namespace Payroll.Domain {
    public class Entity<T> {
        public T Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity<T>;

            if(object.ReferenceEquals(this, other)) return true;
            
            if(this.GetType() != other.GetType()) return false;

            if(Id.ToString() == other.Id.ToString()) return true;

            return false;
        }

        public override int GetHashCode() {
            return (this.GetType().ToString() + this.Id.ToString()).GetHashCode();
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !a.Equals(b);
        }
  }
}
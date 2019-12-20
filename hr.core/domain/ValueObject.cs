using System.Collections.Generic;
using System.Linq;

namespace hr.core.domain {
    /// <summary>
    /// base class for all value objects.
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
    /// </summary>
    public abstract class ValueObject {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj) {
            if(obj == null || GetType() != obj.GetType())
                return false;
            
            var other = obj as ValueObject;
            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();

            while(thisValues.MoveNext() && otherValues.MoveNext()) {
                if(ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null)) {
                    return false;
                }
                if(thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current)) {
                    return false;
                }
            }
            
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
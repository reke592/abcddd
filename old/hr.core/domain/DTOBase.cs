using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace hr.core.domain {
    public class DTOBase<T> : IDTO<T>
    {
        // private static T _empty = (T) Activator.CreateInstance(typeof(T));
        private ValidationContext _ctx;

        // public static T EmptyInstance => _empty;

        public ValidationContext ValidationContext => _ctx ?? (_ctx = new ValidationContext(this));
            
        public T ToModel() {
            return default(T);
        }

        public ICollection<ValidationResult> Errors {
            get {
                var _ctx = this.ValidationContext;
                var results = new Collection<ValidationResult>();
                Validator.TryValidateObject(this, this.ValidationContext, results);
                return results;
            }
        }

    }

}
using System.Collections.Generic;

namespace hr.core.helper {
    public interface IErrorContainer<T> {
        IEnumerable<T> GetErrors { get; }
    }
}
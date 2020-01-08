using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hr.core.helper {
    public interface IValidity {
        ICollection<ValidationResult> Errors { get; }
    }
}
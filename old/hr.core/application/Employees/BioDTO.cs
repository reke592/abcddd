using System.ComponentModel.DataAnnotations;
using hr.core.domain.Employees;
using hr.core.domain.commons;

namespace hr.core.application.Employees {
    public class BioDTO {
        public long Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string ExtName { get; set; }

        [Required]
        public Gender Gender { get; set; }

    }
}
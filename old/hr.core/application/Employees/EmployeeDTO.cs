using System.ComponentModel.DataAnnotations;
using hr.core.domain.Employees;

namespace hr.core.application.Employees {
    public class EmployeeDTO {
        public long Id { get; set; }

        [Required]
        public DepartmentDTO Department { get; set; }

        [Required]
        public BioDTO Bio { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }

    }
}
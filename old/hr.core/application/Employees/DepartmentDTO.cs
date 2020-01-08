using System.ComponentModel.DataAnnotations;

namespace hr.core.application.Employees {
    public class DepartmentDTO {
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int EmployeeCount { get; set; }
        
        [Required]
        public int Capacity { get; set; }
    }
}
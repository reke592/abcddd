using System.ComponentModel.DataAnnotations;
using hr.core.application.commons;

namespace hr.core.application.Employees {
    public class WorkScheduleDTO {
        public long Id { get; set; }
        
        [Required]
        public TimeValueDTO Start { get; set; }

        [Required]
        public TimeValueDTO End { get; set; }
    }
}
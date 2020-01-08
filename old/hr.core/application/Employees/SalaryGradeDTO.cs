using System.ComponentModel.DataAnnotations;
using hr.core.application.commons;

namespace hr.core.application.Employees {
    public class SalaryGradeDTO {
        public long Id { get; set; }
        
        [Required]
        public string Level { get; set; }

        [Required]
        public MonetaryValueDTO Gross { get; set; }
    }
}
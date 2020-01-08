using System.ComponentModel.DataAnnotations;
using hr.core.domain.Employees;

namespace hr.core.application.Employees {
    public class AddressDTO {
        public long Id { get; set; }

        public string LotBlock { get; set; }
        public string Street { get; set; }
        
        [Required]
        public string Municipality { get; set; }
        
        [Required]
        public string Province { get; set; }
    }
}
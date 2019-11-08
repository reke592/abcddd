using System;
using hr.domain.shared;

namespace hr.application.Employees {
    public class PersonDTO {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string Ext { get; set; }
        public string Birthdate { get; set; }
        public string Sex { get; set; }
    }
}
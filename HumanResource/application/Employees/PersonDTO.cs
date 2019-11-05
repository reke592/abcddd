using System;
using hr.domain.shared;

namespace hr.application.Employees {
    public class PersonDTO {
        public string Firstname { get; protected set; }
        public string Middlename { get; protected set; }
        public string Surname { get; protected set; }
        public string Ext { get; protected set; }
        public EnumSex Sex { get; protected set; }
        public DateTime Birthdate { get; protected set; }
    }
}
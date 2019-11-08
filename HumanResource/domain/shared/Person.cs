using System;
using hr.helper.errors;

namespace hr.domain.shared {
    public class Person {
        public virtual Guid Id { get; protected set; }
        public virtual string Firstname { get; protected set; }
        public virtual string Middlename { get; protected set; }
        public virtual string Surname { get; protected set; }
        public virtual string Ext { get; protected set; }
        // public virtual DateTime Birthdate { get; protected set; }
        public virtual EnumSex Sex { get; protected set; }
        public virtual Date Birthdate { get; protected set; }

        public static Person Create(
            string firstname, string middlename, string surname,
            string ext, EnumSex sex, Date birthdate) {
    
            using(var x = new ErrorBag()) {
                x.Required("firstname", firstname).Min(2).Max(20).AlphaSpaces();
                x.Required("surname", surname).Min(2).Max(20).AlphaSpaces();
                x.Required("birthdate", birthdate.ToString()).DateValue();
                x.Optional("middlename", middlename).Min(2).Max(20).AlphaSpaces();
                x.Optional("ext", ext).Min(1).Max(10);
            }

            return new Person {
                Id = Guid.NewGuid(),
                Firstname = firstname,
                Middlename = middlename,
                Surname = surname,
                Ext = ext,
                Sex = sex,
                Birthdate = birthdate
            };
        }
    }
}
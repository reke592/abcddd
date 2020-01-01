using hr.core.domain.shared;

namespace hr.core.domain.Employees {
    public class Bio : Entity {
        public virtual string FirstName { get; protected set; }
        public virtual string MiddleName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string ExtName { get; protected set; }
        public virtual Gender Gender { get; protected set; }
        public virtual Date Birthdate { get; protected set; }
        public virtual Address HomeAddress { get; protected set; }
        public virtual Address PresentAddress { get; protected set; }

        public static Bio Create(
            string firstname, string middlename, string lastname
            , string extname, Gender gender, Date birthdate) {

            return new Bio {
                // Id = Guid.NewGuid(),
                FirstName = firstname,
                MiddleName = middlename,
                LastName = lastname,
                ExtName = extname,
                Gender = gender,
                Birthdate = birthdate
            };
        }  
    }
}
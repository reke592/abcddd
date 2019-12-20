namespace hr.core.domain.shared {
    // it doesn't mean that two people with same identity is one person
    public class Person : Entity {
        // public virtual Guid Id { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string MiddleName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string ExtName { get; protected set; }
        public virtual Gender Gender { get; protected set; }
        public virtual Date Birthdate { get; protected set; }

        public static Person Create(
            string firstname, string middlename, string lastname
            , string extname, Gender gender, Date birthdate) {

            return new Person {
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
using System.Collections.Generic;
using hris.xunit.units.application.Employees;
using hris.xunit.units.domain.ValueObjects;

namespace hris.xunit.units.domain.Employees {
    public class Bio : ValueObject {
        public readonly string FirstName;
        public readonly string MiddleName;
        public readonly string LastName;
        public readonly string DateOfBirth;
        
        public Bio() { }

        public Bio(string firstName, string middleName, string lastName, string dateOfBirth)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            DateOfBirth = Date.TryParse(dateOfBirth, @throw: true).ToString();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return MiddleName;
            yield return LastName;
            yield return DateOfBirth;
        }
    }
}
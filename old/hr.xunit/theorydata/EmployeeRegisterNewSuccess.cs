using hr.com.application.Employees;
using Xunit;

namespace hr.xunit.theorydata {
    public class EmployeeRegisterNewSuccess : TheoryData<PersonDTO> {
        public EmployeeRegisterNewSuccess() {
            Add(new PersonDTO {
                FirstName = "Juan",
                MiddleName = "Cruz",
                LastName = "Dela Cruz",
                Gender = "Male"
            });

            Add(new PersonDTO {
                FirstName = "Mary Ann",
                MiddleName = "Cruz",
                LastName = "Dela Cruz",
                Gender = "Female"
            });
        }
    }
}
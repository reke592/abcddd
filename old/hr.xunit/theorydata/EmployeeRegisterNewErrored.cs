using hr.com.application.Employees;
using Xunit;

namespace hr.xunit.theorydata {
    public class EmployeeRegisterNewErrored : TheoryData<PersonDTO> {
        public EmployeeRegisterNewErrored() {
            Add(new PersonDTO {
                FirstName = "Juan",
                MiddleName = "Cruz",
                LastName = "Dela Cruz",
                Gender = ""
            });

            Add(new PersonDTO {
                FirstName = "Mary Ann",
                MiddleName = "Cruz",
                LastName = "Dela Cruz",
                Gender = "-1"
            });
        }
    }
}
using hr.application.Employees;
using Xunit;

namespace hr.test.theorydata {
    public class CreateEmployeeTestData : TheoryData<PersonDTO> 
    {
        public CreateEmployeeTestData() {
            Add(new PersonDTO {
                Firstname = "Juan"
                , Middlename = "Santos"
                , Surname = "Dela Cruz"
                , Ext = "Jr"
                , Birthdate = "january 1, 2001"
                , Sex = "Male"
            });

            Add(new PersonDTO {
                Firstname = "Ann"
                , Middlename = "Bernabe"
                , Surname = "Cruz"
                , Birthdate = "April 4, 2004"
                , Sex = "Female"
            });
        }
    }
}
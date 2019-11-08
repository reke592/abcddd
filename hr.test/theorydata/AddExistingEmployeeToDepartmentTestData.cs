using hr.application;
using hr.application.Departments;
using Xunit;

namespace hr.test.theorydata {
    public class AddExistingEmployeeToDepartment : TheoryData<application.AddExistingEmployeeToDepartmentRequest> 
    {
        public AddExistingEmployeeToDepartment() {
            Add(new application.AddExistingEmployeeToDepartmentRequest { EmployeeId = 1, DepartmentId = 1 });
            Add(new application.AddExistingEmployeeToDepartmentRequest { EmployeeId = 2, DepartmentId = 1 });
        }
    }
}
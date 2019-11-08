using hr.application.Departments;
using Xunit;

namespace hr.test.theorydata {
    public class CreateDepartmentTestData : TheoryData<DepartmentDTO> 
    {
        public CreateDepartmentTestData() {
            Add(new DepartmentDTO { Name = "Faculty", Capacity = 20 });
            Add(new DepartmentDTO { Name = "Finance", Capacity = 10 });
        }
    }
}
using hr.application.Departments;
using hr.application.Employees;

namespace hr.application {
    public interface IHRApplication {
        object CreateDepartment(DepartmentDTO input);
        object CreateEmployee(PersonDTO input);
        object AssignEmployeeToDepartment(long employeeId, long departmentId);
    }
}
using hr.domain.models.Employees;

namespace hr.domain.models.Companies {
    public interface IDepartmentDomainServices {
        void AssignEmployeeToDepartment(Employee employee, Department department);
        void RemoveEmployeeFromDepartment(Employee employee, Department department);
    }
}
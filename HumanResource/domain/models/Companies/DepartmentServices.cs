using hr.application.Departments;
using hr.domain.models.Employees;
using hr.helper.domain;

namespace hr.domain.models.Companies {
    public class DepartmentDomainServices {
        private readonly IEmployeeRepository _employees;
        private readonly IDepartmentRepository _departments;
        
        public DepartmentDomainServices(IEmployeeRepository employees, IDepartmentRepository departments) {
            this._employees = employees;
            this._departments = departments;
        }

        public Department CreateDepartment(DepartmentDTO input) {
            var department = Department.Create(input.Name, input.Capacity);
            // emit event: Created new department
            return department;
        }

        public void AddEmployeeToDepartment(Department department, Employee employee) {
            department.addEmployee(employee);
            // emit event: Employee added to department
        }
    }
}
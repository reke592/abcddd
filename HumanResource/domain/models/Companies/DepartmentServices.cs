using hr.domain.models.Employees;

namespace hr.domain.models.Companies {
    public class DepartmentServices {
        private readonly IEmployeeRepository _employees;
        private readonly IDepartmentRepository _departments;
        
        public DepartmentServices(IEmployeeRepository employees, IDepartmentRepository departments) {
            this._employees = employees;
            this._departments = departments;
        }

        public void AddEmployeeToDepartment(Department department, Employee employee) {
            department.addEmployee(employee);
            _departments.update(department);
        }
    }
}
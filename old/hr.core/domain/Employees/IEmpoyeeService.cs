using hr.core.application.Employees;
using hr.core.helper;

namespace hr.core.domain.Employees {
    public interface IEmployeeService {
        Employee CreateEmployee(EmployeeDTO input);
        Address CreateAddress(AddressDTO input);
        WorkSchedule CreateWorkSchedule(WorkScheduleDTO input);
        SalaryGrade CreateSalaryGrade(SalaryGradeDTO input);
        Department CreateDepartment(DepartmentDTO input);
    }
}
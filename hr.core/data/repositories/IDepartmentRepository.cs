using System.Collections.Generic;
using hr.core.models;

namespace hr.core.data.repositories
{
  public interface IDepartmentRepository : IRepository<Department>
  {
    Department FindByName(string name);
    IList<Employee> GetEmployeeList(string departmentName);
    void AddNewEmployeeToDepartment(string departmentName, PersonDetails details);
    void RemoveEmployeeInDepartment(string departmentName, Employee employee);
    void MoveEmployeeToAnotherDepartment(Employee employee, string fromDepartment, string toDepartment);
  }
}
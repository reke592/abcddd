using System.Collections.Generic;
using hr.core.data.repositories;
using hr.core.models;

namespace hr.infrastracture.NHibernate.repositories
{
  public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
  {
    public Department FindByName(string name) {
      using (var s = NHibernateHelper.OpenSession()) {
        return s.CreateQuery("from Department where Department.Name = ?")
          .SetString(0, name)
          .UniqueResult<Department>();
      }
    }

    // wait, all interaction executes 1 query at a time?
    public void AddNewEmployeeToDepartment(string departmentName, PersonDetails details) {
      throw new System.NotImplementedException();
    }

    public IList<Employee> GetEmployeeList(string departmentName) {
      throw new System.NotImplementedException();
    }

    public void MoveEmployeeToAnotherDepartment(Employee employee, string fromDepartment, string toDepartment) {
      throw new System.NotImplementedException();
    }

    public void RemoveEmployeeInDepartment(string departmentName, Employee employee) {
      throw new System.NotImplementedException();
    }
  }
}
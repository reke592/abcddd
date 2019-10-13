using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace hr.core.models
{
  public class Department : Entity
  {
    public virtual string Name { get; private set; }
    private List<Employee> _Employees;
    public virtual ICollection<Employee> Employees =>
      new ReadOnlyCollection<Employee>(_Employees);

    public Department() { }

    public Department(string name) {
      this.Name = name;
    }

    public void AddEmployee(PersonDetails pDetails) {
      this._Employees.Add(new Employee(this, pDetails));
    }

    public void RemoveEmployee(Employee employee) {
      this._Employees.Remove(employee);
    }
  }
}
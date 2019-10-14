using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace hr.core.models
{
  /// <summary>
  /// Aggregate Root
  /// </summary>
  public class Department : Entity
  {
    public virtual string Name { get; protected set; }
    private List<Employee> _Employees;

    public virtual ICollection<Employee> Employees {
      get { return new ReadOnlyCollection<Employee>(_Employees); }
      protected set { _Employees = new List<Employee>(value); }
    }

    public Department() { }

    public Department(string name) {
      this.Name = name;
      this._Employees = new List<Employee>();
    }

    /// <summary>
    /// Add new Employee
    /// </summary>
    public virtual void AddEmployee(PersonDetails pDetails) {
      this._Employees?.Add(new Employee(this, pDetails));
    }

    /// <summary>
    /// Add existing employee to this department
    /// </summary>
    public virtual void AddEmployee(Employee employee) {
      this._Employees?.Add(employee);
    }

    /// <summary>
    /// Move employee to another department
    /// </summary>
    public virtual void MoveEmployeeToDepartment(Employee employee, Department other) {
      this._Employees?.Remove(employee);
      other.AddEmployee(employee);
    }

    public virtual void RemoveEmployee(Employee employee) {
      this._Employees.Remove(employee);
    }

    public virtual bool hasEmployee(Employee employee) {
      return this._Employees.Contains(employee);
    }
  }
}
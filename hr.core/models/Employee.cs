using System;
using hr.core.enums;

namespace hr.core.models
{
  public class Employee : Entity
  {
    public virtual PersonDetails PersonDetails { get; protected set; }
    public virtual Department Department { get; protected set; }
    public virtual string Status { get; protected set; }

    public Employee() { }

    /// <summary>
    /// only create new Employee with person details and department
    /// </summary>
    public Employee(Department department, PersonDetails pDetails) {
      this.setEmploymentStatus(EmploymentStatus.NEW_HIRED);
      this.PersonDetails = pDetails;
      this.Department = department;
    }

    public virtual void setEmploymentStatus(EmploymentStatus value) {
      this.Status = Enum.GetName(typeof(EmploymentStatus), value);
    }

    /// seems imposible to have an employee without assignement
    // public virtual void changeDepartment(Department to) {
    //   this.Department?.RemoveEmployee(this);
    //   to.AddEmployee(this);
    // }
  }
}

using System;
using hr.core.enums;

namespace hr.core.models
{
  public class Employee : Entity
  {
    public virtual PersonDetails PersonDetails { get; private set; }
    public virtual string Status { get; private set; }
    public virtual Department Department { get; private set; }

    public Employee() { }

    public Employee(Department department, PersonDetails pDetails) {
      this.Department = department;
      this.PersonDetails = pDetails;
    }

    public void setEmploymentStatus(EmploymentStatus value) {
      this.Status = Enum.GetName(typeof(EmploymentStatus), value);
    }
  }
}

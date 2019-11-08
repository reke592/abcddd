using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.domain.models.Employees;
using hr.helper.domain;
using hr.helper.errors;

namespace hr.domain.models.Companies {
    public class Department : Entity {
        public virtual string Name { get; protected set; }
        public virtual int Capacity { get; protected set; }
        private IList<Employee> employees = new List<Employee>();

        public static Department Create(string name, int capacity) {
            using(var x = new ErrorBag()) {
                x.Required("name", name).AlphaSpaces().Max(30);
                x.Required("capacity", capacity).Min(1).Max(10000);
            }

            return new Department {
                Name = name,
                Capacity = capacity
            };
        }

        public virtual ReadOnlyCollection<Employee> Employees {
            get {
                return new ReadOnlyCollection<Employee>(this.employees);
            }
        }

        public virtual Command addEmployee(Employee employee) {
            if(this.employees.Count == this.Capacity)
                return new Errored(new { Message = $"{this.Name} department capacity is full" });
            if(employee.getDepartment() != null)
                return new Errored(new { Message = $"Employee is currently assigned to {employee.getDepartment().Name}" });

            this.employees.Add(employee);
            return new EmployeeAssignedToDepartment(employee, this);
        }

        public virtual Command removeEmployee(Employee employee) {
            if(!this.employees.Contains(employee))
                return new Errored(new { Message = $"Employee not assigned in {this.Name} department" });

            this.employees.Remove(employee);
            return new EmployeeRemovedFromDepartment(employee, this);
        }
    }
}
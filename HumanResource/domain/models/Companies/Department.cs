using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.domain.models.Employees;
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

        public virtual void addEmployee(Employee employee) {
            // validate capacity
            this.employees.Add(employee);
        }

        public virtual void removeEmployee(Employee employee) {
            this.employees.Remove(employee);
        }
    }
}
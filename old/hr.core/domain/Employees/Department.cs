using hr.core.domain.Employees.events;
using hr.core.domain.Employees.rules;
using hr.core.helper;

namespace hr.core.domain.Employees {
    public class Department : Entity {
        public string Name { get; protected set; }
        public int EmployeeCount { get; protected set; }
        public int Capacity { get; protected set; }

        public bool CanAddEmployee {
            get {
                return new DepartmentCanAddEmployeeRule().isSatisfiedBy(this);
            }
        }

        public static Department Create(string name, int capacity) {
            var record = new Department {
                Name = name,
                Capacity = capacity
            };

            return record;
        }
    }
}
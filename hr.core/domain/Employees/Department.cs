using hr.core.domain.Employees.rules;

namespace hr.core.domain.Employees {
    public class Department : Entity {
        public string Name { get; protected set; }
        public int Capacity { get; protected set; }
        public int Limit { get; protected set; }

        public bool CanAddEmployee {
            get {
                return new DepartmentCanAddEmployeeRule().isSatisfiedBy(this);
            }
        }
    }
}
using hr.core.helper;

namespace hr.core.domain.Employees.events {
    public class DepartmentRecordCreated : Event {
        public Department Department { get; private set; }

        public DepartmentRecordCreated(Department department) {
            Department = department;
        }
    }
}
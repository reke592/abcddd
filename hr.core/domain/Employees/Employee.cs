namespace hr.core.domain.Employees {
    // Aggregate Root
    public class Employee : Entity {
        private long _bio_id;
        private long _salary_grade_id;
        private long _work_schedule_id;
        private long _department_id;

        public virtual Bio Bio { get; protected set; }
        public virtual EmployeeStatus Status { get; protected set; }

        public bool IsActive {
            get { 
                return new ActiveStatusRule().isSatisfiedBy(this);
            }
        }

        // public void setDepartment(Department department) {
        //     if(department.CanAddEmployee) {
        //         _department_id = department.Id;
        //         // emit command AddEmployeeToDepartment(this, department)
        //     }
        // }

        // public void leaveDepartment() {
        //     var current_department = _department_id;   
        //     _department_id = 0;
        //     // emit event EmployeeHasLeaveTheDepartment(this, current_department)
        // }

        // public void changeStatus(EmployeeStatus status) {
        //     var current_status = Status;
        //     Status = status;
        //     // emit event EmployeeStatusChanged(this, current_status)
        // }
    }
}
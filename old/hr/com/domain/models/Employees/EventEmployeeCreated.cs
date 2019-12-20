namespace hr.com.domain.models.Employees {
    public class EventEmployeeCreated : helper.domain.Event
    {
        public Employee Employee { get; protected set; }

        public EventEmployeeCreated(Employee employee) {
            this.Employee = employee;
        }
    }
}
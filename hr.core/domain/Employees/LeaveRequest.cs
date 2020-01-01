using hr.core.domain.shared;

namespace hr.core.domain.Employees {
    public class LeaveRequest : Entity {
        private long _employee_id;

        public RequestStatus Status { get; protected set; }
        public int TotalDays { get; protected set; }
    }
}
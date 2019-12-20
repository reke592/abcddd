using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventSalaryDeductionCreated : Event {
        public Salary Salary { get; protected set; }
        public Deduction Deduction { get; protected set; }
        public EventSalaryDeductionCreated(Deduction deduction, Salary salary) {
            this.Deduction = deduction;
            this.Salary = salary;
        }
    }
}
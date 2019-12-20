using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class EventSalaryDeductionAdded : Event {
        public Deduction Deduction { get; protected set; }
        public Salary Salary { get; protected set; }

        public EventSalaryDeductionAdded(Salary salary, Deduction deduction) {
            this.Salary = salary;
            this.Deduction = deduction;
        }
    }
}
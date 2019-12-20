using hr.com.helper.domain;

namespace hr.com.domain.models.Payrolls {
    public class CommandAddSalaryDeduction : Command {
        public Salary Salary { get; protected set; }
        public Deduction Deduction { get; protected set; }

        public CommandAddSalaryDeduction(Salary salary, Deduction deduction) {
            this.Salary = salary;
            this.Deduction = deduction;
        }
    }
}
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Payrolls;
using hr.com.domain.shared;
using Xunit;

namespace hr.xunit.units.Payrolls {
    public class DeductionTest {
        private static Employee employee = Employee.Create(Person.Create("a", "a", "a", "", Gender.MALE, Date.Now), Date.Now);
        private static Salary salary = Salary.Create(employee, MonetaryValue.of("PHP", 15000));

        [Fact]
        public void can_add_deduction() {
            var d = Deduction.Create(salary, 3, MonetaryValue.of("php", 1000));
            Assert.Contains<Deduction>(d, salary.Deductions);
        }

        [Fact]
        public void default_payment_for_deduction_is_the_amortized_amount() {
            var d = Deduction.Create(salary, 3, MonetaryValue.of("php", 1000));
            var p = DeductionPayment.Create(d);
            Assert.Equal(d.AmortizedAmount.DecimalValue(), p.PaidAmount.DecimalValue());
        }

        [Fact]
        public void deduction_amortization_adjusted_when_custom_payment_was_made() {
            var d = Deduction.Create(salary, 3, MonetaryValue.of("php", 1000));
            var actual = d.AmortizedAmount.DecimalValue();

            var p = DeductionPayment.Create(d, MonetaryValue.of("php", 700));

            Assert.Equal(d.Balance.DecimalValue(), 300);
            Assert.Equal(d.Paid.DecimalValue(), 700);
            Assert.Equal(d.Total.subtractValueOf(d.Paid).DecimalValue(), d.Balance.DecimalValue());
            Assert.Equal(d.AmortizedAmount.DecimalValue(), (1000 - 700) / (3 - 1));
            Assert.NotEqual(d.AmortizedAmount.DecimalValue(), actual);
        }
    }
}
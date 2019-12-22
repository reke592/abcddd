using System;
using System.Linq.Expressions;
using hr.core.domain.Employees;
using hr.core.infrastracture;

namespace hr.core.domain.Payrolls {
    // CanAddSallaryDeductionRule(employee, salary).satisfiedBy(deduction)
    public class CanAddSalaryDeductionRule : Specification<Deduction>
    {
        private Employee _employee;
        private Salary _salary;
        
        public CanAddSalaryDeductionRule(Employee employee, Salary salary) {
            this._employee = employee;
            this._salary = salary;
        }

        public override Expression<Func<Deduction, bool>> toExpression()
        {
            return candidate => 
                _employee.IsActive
                && candidate.CanBeAdded
                && candidate.AmortizedAmount.PreciseValue <= _salary.Net.PreciseValue;
        }
    }
}
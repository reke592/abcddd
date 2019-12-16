using System;
using System.Linq.Expressions;
using hr.com.domain.models.Employees;
using hr.com.helper.database;

namespace hr.com.domain.models.Payrolls.specs {
    public class SpecificationActiveEmployeeDeduction : Specification<Deduction>
    {
        private Employee _reference;

        public SpecificationActiveEmployeeDeduction(Employee employee) {
            this._reference = employee;
        }

        public override Expression<Func<Deduction, bool>> toExpression()
        {
            return x => x.hasBalance && (x.Employee.Id == this._reference.Id);
        }
    }
}
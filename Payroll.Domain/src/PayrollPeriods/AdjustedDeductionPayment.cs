using System.Collections.Generic;
using Payroll.Domain.Deductions;
using Payroll.Domain.Employees;

namespace Payroll.Domain.PayrollPeriods
{
  public class AdjustedDeductionPayment : ValueObject
  {
    public EmployeeId Employee { get; private set; }
    public DeductionId Deduction { get; private set; }
    public decimal PaidAmount { get; private set; }

    public static AdjustedDeductionPayment Create(EmployeeId employee, DeductionId deduction, decimal paidAmount)
    {
      return new AdjustedDeductionPayment {
        Employee = employee,
        Deduction = deduction,
        PaidAmount = paidAmount
      };
    }

    protected override IEnumerable<object> GetAtomicValues() {
      yield return Deduction;
      yield return PaidAmount;
    }
  }
}
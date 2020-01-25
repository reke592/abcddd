using System.Collections.Generic;

namespace Payroll.Domain.PayrollPeriods
{
  public class PayrollConsignee : ValueObject
  {
    public string Name { get; private set; }
    public string Position { get; private set; }
    public string Role { get; private set; }

    public static PayrollConsignee Create(string name, string position, string role)
    {
      return new PayrollConsignee {
        Name = name,
        Position = position,
        Role = role
      };
    }

    protected override IEnumerable<object> GetAtomicValues() {
      yield return Name;
      yield return Position;
      yield return Role;
    }
  }
}
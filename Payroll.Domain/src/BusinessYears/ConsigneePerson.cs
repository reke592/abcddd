using System;
using System.Collections.Generic;
using Payroll.Domain.Users;

namespace Payroll.Domain.BusinessYears
{
  public class ConsigneePerson : ValueObject
  {
    public string Name { get; private set; }
    public string Position { get; private set; }

    public static ConsigneePerson Create(string name, string position)
    {
      return new ConsigneePerson {
        Name = name,
        Position = position
      };
    }

    protected override IEnumerable<object> GetAtomicValues() {
      yield return Name;
      yield return Position;
    }
  }
}
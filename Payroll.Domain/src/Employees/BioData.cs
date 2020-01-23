using System.Collections.Generic;
using Payroll.Domain.Shared;

namespace Payroll.Domain.Employees
{
  public class BioData : ValueObject
  {
    public string Firstname { get; private set; }
    public string Middlename { get; private set; }
    public string Surname { get; private set; }
    public string DateOfBirth { get; private set; }

    public static BioData Create(string firstname, string middlename, string surname, Date dateOfBirth)
    {
      return new BioData {
        Firstname = firstname,
        Middlename = middlename,
        Surname = surname,
        DateOfBirth = dateOfBirth.ToString()
      };
    }

    protected override IEnumerable<object> GetAtomicValues() {
      yield return Firstname;
      yield return Middlename;
      yield return Surname;
      yield return DateOfBirth;
    }
  }
}
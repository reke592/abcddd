using System;
using hr.domain.shared;
using hr.helper.errors;

namespace console
{
   class Program
  {
    static void Main(string[] args) {
      try {
        var p = Person.Create("Erric John", "Castillo", "Rapsing", "Ir", EnumSex.Male, new DateTime(1992,5,24));
      }
      catch(ErrorBag errors) {
        Console.WriteLine(errors.Count);
        foreach(var k in errors.Errors.Keys) {
          foreach(var m in errors.Errors[k]) {
            Console.WriteLine($"{k} {m}");
          }
        }
      }
    }
  }
}

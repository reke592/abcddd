using System;
using System.Linq.Expressions;
using hr.com.domain.enums;
using hr.com.domain.models.Employees;
using hr.com.domain.models.Employees.specs;
using hr.com.domain.models.Payrolls;
using hr.com.domain.models.Payrolls.specs;
using hr.com.domain.shared;
using hr.com.helper.database;
using hr.com.helper.domain;
using hr.com.infrastracture.database.nhibernate;
using hr.com.infrastracture.database.nhibernate.repositories;

namespace console
{
  class Program
  {
    static readonly IRepository<Salary> _salaries = new SalaryRepository();
    static readonly IRepository<Employee> _employees = new EmployeeRepository();

    static void Main(string[] args) {
      using(var uow = new NHUnitOfWork()) {
        var p = Person.Create("Sam", "Wilson", "Tucker", "", Gender.MALE, Date.TryParse("September 11, 2019"));
        var e = Employee.Create(p, Date.Now, EmployeeStatus.RETIRED);
        var s = Salary.Create(e, MonetaryValue.of("php", 25000m));
        var d = Deduction.Create(s, 12, MonetaryValue.of("php", 12000m));
        uow.Session.Save(e);
        uow.Session.Save(s);
        uow.Commit();
      }

      using(var uow = new NHUnitOfWork()) {
        var p = Person.Create("Juan", "Cruz", "Dela Cruz", "Jr", Gender.MALE, Date.TryParse("September 11, 2019"));
        var e = Employee.Create(p, Date.Now);
        var s = Salary.Create(e, MonetaryValue.of("php", 25000m));
        var d = Deduction.Create(s, 1, MonetaryValue.of("php", 12000m));
        uow.Session.Save(e);
        uow.Session.Save(s);
        uow.Commit();
      }

      using(var uow = new NHUnitOfWork()) {
        var p = Person.Create("Ann", "Santos", "Abe", "", Gender.FEMALE, Date.TryParse("Feb 29, 2019"));
        var e = Employee.Create(p, Date.Now);
        var s = Salary.Create(e, MonetaryValue.of("php", 27000m));
        var d = Deduction.Create(s, 12, MonetaryValue.of("php", 7000m));
        uow.Session.Save(e);
        uow.Session.Save(s);
        uow.Commit();
      }

      Console.WriteLine("\n\n");
      using(var uow = new NHUnitOfWork()) {
        var activeEmployees = new EmployeeIsActive();
        var ees = _employees.FindAll(activeEmployees);
        var report = PayrollReport.Create(ees, Date.Now);
        EventBroker.getInstance().Command(new CommandIncludeSalaryDeductionInReport(report));
        
        uow.Session.Save(report);
        uow.Commit();

        foreach(var r in report.Records) {
          Console.WriteLine(r);
        }
      }

      Console.WriteLine("\n\n");
      using(var uow = new NHUnitOfWork()) {
        var activeEmployees = new EmployeeIsActive();
        var ees = _employees.FindAll(activeEmployees);
        var report = PayrollReport.Create(ees, Date.Now);
        EventBroker.getInstance().Command(new CommandIncludeSalaryDeductionInReport(report));
        
        uow.Session.Save(report);
        uow.Commit();

        foreach(var r in report.Records) {
          Console.WriteLine(r);
        }
      }
    }
  }
}

//  var app = new HRApplicationTest();
//       var e1 = app.CreateEmployee(new PersonDTO {
//           Firstname = "Juan"
//           , Middlename = "Santos"
//           , Surname = "Dela Cruz"
//           , Ext = "Jr"
//           , Birthdate = "january 1, 2001"
//           , Sex = "Male"
//       }) as Employee;

//       var e2 = app.CreateEmployee(new PersonDTO {
//           Firstname = "Ann"
//           , Middlename = "Bernabe"
//           , Surname = "Cruz"
//           , Birthdate = "April 4, 2004"
//           , Sex = "Female"
//       }) as Employee;

//       var d1 = app.CreateDepartment(new DepartmentDTO{ Name="Faculty", Capacity=20 }) as Department;
//       var d2 = app.CreateDepartment(new DepartmentDTO{ Name="Finance", Capacity=10 }) as Department;

//       Console.WriteLine($"employee1: {e1.Id}");
//       Console.WriteLine($"employee2: {e2.Id}");
//       Console.WriteLine($"department1: {d1.Id}");
//       Console.WriteLine($"department2: {d2.Id}");

//       app.AssignEmployeeToDepartment(e1.Id, d1.Id);
//       app.AssignEmployeeToDepartment(e2.Id, d1.Id);

//       using(var uow = new NHUnitOfWork()) {
//         var q = uow.Session.CreateCriteria(typeof(Employee))
//           .Fetch(SelectMode.Fetch, "department")
//           .Add(Expression.Eq("Id", 1L))
//           .UniqueResult<Employee>();
        
//         Console.WriteLine(q.getDepartment().Name);
//       }



 // // TestAsyncNHSession();
      // var details = new PersonDTO {
      //   Firstname = "Erric",
      //   Surname = "Rapsing",
      //   Middlename = "Castillo",
      //   Ext = "",
      //   Birthdate = "5/24/1992",
      //   Sex = "Male"
      // };

      // var emp_repo = new EmployeeRepository();
      // var dept_repo = new DepartmentRepository();
      // var emp_service = new EmployeeServices(emp_repo);
      // var dept_service = new DepartmentServices(emp_repo, dept_repo);

      // using(var uow = new NHUnitOfWork()) {
      //   // emp_service.CreateEmployee(details);
      //   var e = Employee.Create(Person.Create(
      //     details.Firstname
      //     , details.Middlename
      //     , details.Surname
      //     , details.Ext
      //     , (EnumSex) Enum.Parse(typeof(EnumSex), details.Sex)
      //     , Date.TryParse(details.Birthdate)
      //   ));
      //   var d = Department.Create("Finance", 10);

      //   // dept_repo.Save(d);
      //   emp_repo.Save(e);
      //   dept_service.AddEmployeeToDepartment(d, e);
      //   uow.Commit();
      // }

      // // using(var uow = new NHUnitOfWork()) {
      // //   var e = uow.Session.Get<Employee>(1L);
      // //   var d = uow.Session.Get<Department>(1L);
      // //   dept_service.AddEmployeeToDepartment(d, e);
      // //   uow.Commit();
      // // }

      // using(var uow = new NHUnitOfWork()) {
      //   IQueryable<Employee> query = from e in uow.Session.Query<Employee>() 
      //       let name = (from p in uow.Session.Query<Person>() select p.Firstname)
      //       where name.Any(x => x == "Erric")
      //       select e;

      //   var r = query.First();
      //   var d = r.getPersonDetails().Birthdate;
      //   Console.WriteLine($"{d.LongMonth} {d.Day}, {d.Year}");
      //   Console.WriteLine(r.getDepartment()?.Name);
      // }

using System;
using hr.com.application.Employees;
using hr.com.domain.models.Employees;
using hr.com.domain.shared;
using hr.com.domain.enums;
using hr.com.helper.database;

namespace hr.xunit.units {
    public class EmployeeAppService : IEmployeeAppService
    {
        private static readonly IEmployeeDomainService _EMP_DOMAIN = new EmployeeDomService();
        private static IRepository<Employee> _employees = new DummyRepository<Employee>();
        public object RegisterNew(PersonDTO data)
        {
            if(!Enum.IsDefined(typeof(Gender), data.Gender.ToUpper()))
                throw new Exception("Invalid Gender");

            if(Date.TryParse(data.Birthdate) is null)
                throw new Exception("Invalid Birthdate");

            using(var work = new DummyUnitOfWork()) {
                var person = Person.Create(data.FirstName
                    , data.MiddleName
                    , data.LastName
                    , data.ExtName
                    , (Gender) Enum.Parse(typeof(Gender), data.Gender.ToUpper())
                    , Date.TryParse(data.Birthdate));

                var record = _EMP_DOMAIN.RegisterNew(person);
                _employees.Save(record);
                work.Commit();
                
                return record;
            }
        }
    }
}
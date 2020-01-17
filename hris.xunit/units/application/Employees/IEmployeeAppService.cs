using System.Collections.Generic;
using hris.xunit.units.application.Employees.Projections;
using hris.xunit.units.domain.Employees;

namespace hris.xunit.units.application.Employees
{
    public interface IEmployeeAppService
    {
        List<ActiveEmployeeDocument> GetActiveEmployees();
        EmployeeId AddEmployee(Contracts.V1.CreateEmployeeCommand command);
        ActiveEmployeeDocument ActivateEmployee(Contracts.V1.ActivateEmployeeCommand command);
    }
}
using hr.domain.models.Employees;

namespace hr.infrastracture.database.nhibernate {
    public class EmployeeRepository : NHRepositoryBase<Employee>, IEmployeeRepository {
    }
}
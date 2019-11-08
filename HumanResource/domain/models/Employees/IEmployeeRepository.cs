using hr.helper.database;

namespace hr.domain.models.Employees {
    public interface IEmployeeRepository : IRepository<Employee> {
        Employee findById(long id);
    }
}
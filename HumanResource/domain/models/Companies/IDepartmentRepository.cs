using hr.helper.database;

namespace hr.domain.models.Companies {
    public interface IDepartmentRepository : IRepository<Department> {
        Department findById(long id);
    }
}
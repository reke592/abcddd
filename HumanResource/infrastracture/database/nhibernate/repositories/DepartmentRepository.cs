using hr.domain.models.Companies;

namespace hr.infrastracture.database.nhibernate {
    public class DepartmentRepository : NHRepositoryBase<Department>, IDepartmentRepository
    {
        public Department findById(long id)
        {
            var session = NHibernateHelper.GetCurrentSession();
            return session.Get<Department>(id);
        }
    }
}